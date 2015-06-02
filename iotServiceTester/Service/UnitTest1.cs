using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;

namespace iotServiceTester.Service
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string tokenString = "";

            JwtSecurityToken tokenReceived = new JwtSecurityToken(tokenString);



            //RSACryptoServiceProvider publicOnly = new RSACryptoServiceProvider();
            //publicOnly.FromXmlString(keyGenerationResult.PublicKeyOnly);
            //TokenValidationParameters validationParameters = new TokenValidationParameters()
            //{
            //    ValidIssuer = "http://issuer.com"
            //    ,
            //    AllowedAudience = "http://mysite.com"
            //    ,
            //    SigningToken = new RsaSecurityToken(publicOnly)
            //};

            //JwtSecurityTokenHandler recipientTokenHandler = new JwtSecurityTokenHandler();
            //ClaimsPrincipal claimsPrincipal = recipientTokenHandler.ValidateToken(tokenReceived, validationParameters);
        
        }
    }
}
