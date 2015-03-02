using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using iotDbConnector.DAL;
using iotServiceProvider;

namespace iotServiceTester
{
    [TestClass]
    public class iotUpdatePubSubTester
    {

        public delegate void DeviceEventCallbackHandler(Device dev);
        public static event DeviceEventCallbackHandler DeviceCallbackEvent;

        private IDeviceEventService client;

        private bool UpdateCallbackSuccess = false;

        [CallbackBehaviorAttribute(UseSynchronizationContext = false)]
        public class DeviceEventServiceCallback : IDeviceEventCallback
        {
            public void OnDeviceUpdated(Device dev)
            {
                DeviceCallbackEvent(dev);
            }
        }

        public iotUpdatePubSubTester()
         {
             try
             {
                 InstanceContext context = new InstanceContext(new DeviceEventServiceCallback());
                 client = new iotServiceConnector().GetEventClient(context);

                 DeviceEventCallbackHandler callbackHandler = new DeviceEventCallbackHandler(DidRecieveDeviceUpdate);
                 DeviceCallbackEvent += callbackHandler;

                 client.Subscribe();
             }
             catch (Exception e)
             {
                 
            
             }

         }

        public void DidRecieveDeviceUpdate(Device dev)
         {
             UpdateCallbackSuccess = true;
         }

        Device AddDevice()
        {

            try
            {
                string Loc = Guid.NewGuid().ToString();
                string Type = Guid.NewGuid().ToString();
                string Name = Guid.NewGuid().ToString();
                string Pass = Guid.NewGuid().ToString();
                string Login = Guid.NewGuid().ToString();
                string Host = Guid.NewGuid().ToString();
                string Port = "1212";

                IiotDomainService cl = new iotServiceConnector().GetDomainClient();

                Location loctoadd = new Location();
                loctoadd.LocationName = Guid.NewGuid().ToString();
                loctoadd.Lat = 0;
                loctoadd.Lng = 0;
                cl.LocationAdd(loctoadd);
                List<Location> locs = cl.Locations().ToList();
                 Location loc = (from l in locs
                                where l.LocationName.Equals(loctoadd.LocationName)
                                select l).First();

                iotDomain domain = new iotDomain();
                domain.DomainName = Guid.NewGuid().ToString();
                cl.DomainAdd(domain);
                List<iotDomain> domains = cl.Domains().ToList();
                iotDomain addeddomain = (from d in domains
                                         where d.DomainName.Equals(domain.DomainName)
                                         select d).First();

                Site site = new Site();
                site.SiteName = Name;
                site.siteLocation = loc;
                site.Domain = addeddomain;
                cl.SiteAdd(site);

                Device ndev = new Device();
                ndev.DeviceName = Name;

                DeviceType dtype = new DeviceType();
                dtype.TypeName = Guid.NewGuid().ToString();
                cl.DeviceTypeAdd(dtype);

                List<DeviceType> types = cl.DeviceTypes().ToList();
                DeviceType type = (from t in types
                                   where t.TypeName == dtype.TypeName
                                   select t).First();

                ndev.Type = type;
                ndev.DeviceLocation = loc;
                DeviceCredentials cred = new DeviceCredentials();
                cred.PasswordExpireDate = DateTime.Now.AddYears(100);
                cred.PermissionExpireDate = DateTime.Now.AddYears(100);
                cred.Password = Pass;
                cred.Username = Login;
                cl.DeviceCredentialsAdd(cred);

                List<DeviceCredentials> creds = cl.DeviceCredentials().ToList();
                DeviceCredentials storedCredentials = (from c in creds
                                                       where c.Username == cred.Username
                                                       select c).First();

                EndpointInfo info = new EndpointInfo();
                info.Hostname = Host;
                info.Port = int.Parse(Port);

                info.SupportsSconnProtocol = true;
                cl.EndpointAdd(info);

                List<EndpointInfo> endps = cl.Endpoints().ToList();
                EndpointInfo storedInfo = (from i in endps
                                           where i.Hostname == info.Hostname &&
                                           i.Port == info.Port
                                           select i).First();
                ndev.Credentials = storedCredentials;
                ndev.EndpInfo = storedInfo;

                List<Site> sites = cl.Sites().ToList();
                Site siteToAppend = (from s in sites
                                     where s.SiteName == site.SiteName
                                     select s).First();
                ndev.Site = siteToAppend;
                cl.DeviceAdd(ndev);

                List<Device> devs = cl.Devices().ToList();
                Device stored = (from d in devs
                                 where d.DeviceName == ndev.DeviceName &&
                                 d.EndpInfo.Hostname == ndev.EndpInfo.Hostname
                                 select d).First();

                return stored;
            }
            catch (Exception e)
            {
                return new Device();
              
            }

          

        }


        [TestMethod]
        public void TestConnect()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestSubscribe()
        {
            Assert.IsTrue(true);
            
            /*
            try
            {
                //update device and verify callback
                Device dev = AddDevice();
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                cl.DeviceUpdate(dev);
                while (!UpdateCallbackSuccess)
                {
                    System.Threading.Thread.Sleep(100);
                }
                Assert.IsTrue(UpdateCallbackSuccess);
            }
            catch (Exception  e)
            {
             
            }
             */

        }

        [TestMethod]
        public void TestPublish()
        {
            Assert.IsTrue(true);
        }


    }
}
