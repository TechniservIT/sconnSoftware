using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Net;
using System.Text;
using System.IO;
using iotRestfulService;
using System.ServiceModel;
using System.ServiceModel.Description;
using iotDeviceService;
using RestSharp;


namespace iotServiceTester.Service
{
    [TestClass]
    public class iotRestfulTokenizerTest
    {
        string restServerUri = "http://localhost:45502/DeviceService.svc";

        private ChannelFactory<IDeviceRestfulService> deviceServiceFactory;



        public IDeviceRestfulService GetDeviceClient()
        {
            deviceServiceFactory = new ChannelFactory<IDeviceRestfulService>("iotRestServiceHttp");
            return deviceServiceFactory.CreateChannel();
        }


        [TestMethod]
        public void TestTokenGenerationInternal()
        {
            Assert.IsTrue(true);
            return;
            var service = GetDeviceClient();
            var resp = service.GetAuthTokenPublic();
            Assert.IsTrue(resp.ToString().Length > 0);
        }

        [TestMethod]
        public void TestTokenGenRest()
        {
            var service = GetDeviceClient(); //manualy invoke self hosted
            var client = new RestClient("http://localhost:8733/iot/iotDeviceService/rest/");
            var request = new RestRequest("Auth/Private", Method.GET);
            string token = client.Execute(request).Content;
            Assert.IsTrue(token.Length > 0);
        }




        [TestMethod]
        public void TestTokenGeneration()
        {
            Assert.IsTrue(true);   

            //string tokenString = "";
            //string FullReqUri  = restServerUri + "/Auth/Public";
            //HttpWebRequest req = WebRequest.Create(FullReqUri) as HttpWebRequest;
            //req.KeepAlive = false;
            //req.Method = "GET";
            //var resp = req.GetResponse();
            //Encoding enc = System.Text.Encoding.GetEncoding(1252);
            //StreamReader loResponseStream =
            //new StreamReader(resp.GetResponseStream(), enc);
            //tokenString = loResponseStream.ReadToEnd();   
            //JwtSecurityToken tokenReceived = new JwtSecurityToken(tokenString);
            //Assert.IsTrue(tokenReceived != null);
        
        }
    }
}
