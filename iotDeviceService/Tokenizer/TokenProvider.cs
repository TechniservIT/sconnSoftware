using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel;
using System.Security.Cryptography;
using System.IdentityModel.Tokens;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Claims;
using MongoDB.Driver;
using MongoDB.Bson;
using NLog;

namespace iotRestfulService.Security.Tokenizer
{
    public class TokenProvider
    {
        private Logger _logger;

        public TokenProvider()
        {
            sharedKey = Guid.NewGuid().ToString();
             _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        private string sharedKey;

        private void StoreTokenInDb(string tokenStr)
        {
            try
            {
                //store in DB
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("DeviceTokens");
                
                //var document = new BsonDocument("token", tokenString);
                //Task crtask = database.CreateCollectionAsync("tokens");
                //crtask.Wait();

                var collection = database.GetCollection<BsonDocument>("tokens");
                var document = new BsonDocument { { "_id", tokenStr } };
                var doctask = collection.InsertOneAsync(document);
                doctask.Wait();

                //var collection = database.GetCollection<string>("tokens");
                //var task = collection.InsertOneAsync(tokenStr);
                //task.Wait();

            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);  
            }
        }

        private void StoreUserTokenInDb(string user, string tokenStr)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("DeviceTokens");

                var collection = database.GetCollection<BsonDocument>("tokens");
                var document = new BsonDocument { { "user", user } , {"tokendata",tokenStr}, { "tstamp", DateTime.Now.Ticks} };
                var doctask = collection.InsertOneAsync(document);
                doctask.Wait();

            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
            }
        }

        public string CreateAuthenticationTokenMs()
        {
            try
            {
                RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
                RsaKeyGenerationResult keyGenerationResult = GenerateRsaKeys();

                publicAndPrivate.FromXmlString(keyGenerationResult.PublicAndPrivateKey);
                JwtSecurityToken jwtToken = new JwtSecurityToken
                    (issuer: "http://localhost"
                    , audience: "http://localhost"
                    , claims: new List<System.Security.Claims.Claim>() { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "", "http://localhost") }
                    , notBefore: DateTime.Now, expires: DateTime.Now.AddDays(1.0)
                    //,lifetime: new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(1))
                    , signingCredentials: new SigningCredentials(new RsaSecurityKey(publicAndPrivate), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                string tokenString = tokenHandler.WriteToken(jwtToken);
                StoreTokenInDb(tokenString);

                return tokenString;             
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return null;
            }

        }


        public string CreateAuthenticationTokenAuth(string user)
        {
            try
            {
                RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
                RsaKeyGenerationResult keyGenerationResult = GenerateRsaKeys();

                publicAndPrivate.FromXmlString(keyGenerationResult.PublicAndPrivateKey);
                JwtSecurityToken jwtToken = new JwtSecurityToken
                    (issuer: "http://localhost"
                    , audience: "http://localhost"
                    , claims: new List<System.Security.Claims.Claim>() { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "", "http://localhost") }
                    , notBefore: DateTime.Now, expires: DateTime.Now.AddDays(1.0)
                    , signingCredentials: new SigningCredentials(new RsaSecurityKey(publicAndPrivate), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                string tokenString = tokenHandler.WriteToken(jwtToken);
                StoreTokenInDb(tokenString);

                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return null;
            }

        }



        public class RsaKeyGenerationResult
        {
            public string PublicKeyOnly { get; set; }
            public string PublicAndPrivateKey { get; set; }
        }

        private static RsaKeyGenerationResult GenerateRsaKeys()
        {
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider(2048);
            RSAParameters publicKey = myRSA.ExportParameters(true);
            RsaKeyGenerationResult result = new RsaKeyGenerationResult();
            result.PublicAndPrivateKey = myRSA.ToXmlString(true);
            result.PublicKeyOnly = myRSA.ToXmlString(false);
            return result;
        }


    }



}
