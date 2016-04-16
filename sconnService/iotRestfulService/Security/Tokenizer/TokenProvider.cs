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

namespace iotRestfulService.Security.Tokenizer
{
    public class TokenProvider
    {
        public TokenProvider()
        {
            sharedKey = Guid.NewGuid().ToString();
        }

        private string sharedKey;

        public string CreateAuthenticationToken()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int timestamp = (int)t.TotalSeconds;

            var payload = new Dictionary<string, object>
            {
                { "iat", timestamp },
                { "jti", Guid.NewGuid().ToString() },
                { "sharedkey", this.sharedKey }
            };

            string token = JWT.JsonWebToken.Encode(payload, this.sharedKey, JWT.JwtHashAlgorithm.HS256);
            return token;
        }

        public string CreateAuthenticationTokenMs()
        {
            RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
            RsaKeyGenerationResult keyGenerationResult = GenerateRsaKeys();

            publicAndPrivate.FromXmlString(keyGenerationResult.PublicAndPrivateKey);
            JwtSecurityToken jwtToken = new JwtSecurityToken
                (issuer: "http://localhost"
                ,audience: "http://localhost"
                ,claims: new List<System.Security.Claims.Claim>() { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "","http://localhost") }
                , notBefore: DateTime.Now, expires: DateTime.Now.AddDays(1.0)
                  //,lifetime: new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddHours(1))
                ,signingCredentials: new SigningCredentials(new RsaSecurityKey(publicAndPrivate) , SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(jwtToken);

            //Console.WriteLine("Token string: {0}", tokenString);
            return tokenString;
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
