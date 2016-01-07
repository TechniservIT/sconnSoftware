using AlarmSystemManagmentService;
using iotServiceTester.DeviceTypes.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config;
using NUnit.Framework;
using sconnConnector.POCO;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn.IO;
using Assert = NUnit.Framework.Assert;

namespace iotServiceTester.DeviceTypes.Alarm
{

    public static class FakeAlarmServiceFactory
    {
        public static Device GetAlarmDevice()
        {
            Device dev = new Device();
            dev.EndpInfo = new EndpointInfo();
            dev.Credentials = new DeviceCredentials();
            return dev;
        }

        public static sconnAlarmSystem GetFakeAlarmSconnAlarmSystem()
        {
            sconnAlarmSystem sys = new sconnAlarmSystem();
            var fakeables = sys.GetType().GetProperties();
            foreach (PropertyInfo prop in fakeables)
            {
                try
                {
                    if (prop.PropertyType.GetInterfaces().Contains(typeof(IFakeAbleConfiguration)) ) //typeof(IFakeAbleConfiguration).IsAssignableFrom(typeof(prop.)))  //fakeAblePropType
                    {
                        MethodInfo minfo = prop.PropertyType.GetMethod("Fake");
                        var imp = prop.GetValue(sys, null);
                        minfo?.Invoke(imp, null);
                    }
                }
                catch (Exception e)
                {

                }
            }
            return sys;
        }

        public static AlarmSystemConfigManager GetAlarmConfigManager()
        {
            try
            {
                Device dev = FakeAlarmServiceFactory.GetAlarmDevice();
                AlarmSystemConfigManager man = new AlarmSystemConfigManager(dev.EndpInfo, dev.Credentials);
                man.Config = FakeAlarmServiceFactory.GetFakeAlarmSconnAlarmSystem();
                return man;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static IAlarmSystemConfigurationService<T> GetAlarmService<T>()
        {
            AlarmSystemConfigManager man = FakeAlarmServiceFactory.GetAlarmConfigManager();
            IAlarmSystemConfigurationService<T> service;
            if (typeof(T) == typeof(sconnDevice))
            {
                service = new DeviceConfigService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof (T) == typeof (sconnAlarmZone))
            {
                service = new ZoneConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnUser))
            {
                service = new UsersConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnGsmRcpt))
            {
                service = new GsmConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnAuthorizedDevice))
            {
                service = new AuthorizedDevicesConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else
            {
                service =  null;
            }
            service.Online = false; //Disable online config sync for testing
            return service;
        }
}


    public abstract class AlarmServiceDeviceTests<T> where T : new()
    {
        private IAlarmSystemConfigurationService<T> _service;

        [SetUp]
        public void  SetUp()
        {
            _service = FakeAlarmServiceFactory.GetAlarmService<T>();
        }

        [Test]
        public void Test_AlarmService_GetAll()
        {
            var res = _service.GetAll();
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_GetById()
        {
            var res = _service.GetById(0);
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_Add() 
        {
            var res = _service.Add(new T());
            Assert.IsTrue(res);
        }

        [Test]
        public void Test_AlarmService_Edit()
        {

        }

        [Test]
        public void Test_AlarmService_Remove()
        {
            T proto = new T();
            _service.Add(proto);
            var res = _service.Remove(proto);
            var ResCont = _service.GetAll();
            Assert.IsTrue(res && !ResCont.Contains(proto));
        }

        [Test]
        public void Test_AlarmService_RemoveById()
        {
            T proto = new T();
            int RandomId = 156164831;
            PropertyInfo prop = proto.GetType().GetProperty("Id");
            if ((prop) != null)
            {
                prop.SetValue(proto, RandomId);
                _service.Add(proto);
                var res = _service.RemoveById(RandomId);
                var ResCont = _service.GetAll();
                Assert.IsTrue(res && !ResCont.Contains(proto));
            }
            else
            {
                Assert.IsTrue(false);
            }

        }

        [Test]
        public void Test_AlarmService_Update()
        {
            T proto = new T();
            int RandomId = 156164831;   //Test updating by Id property
            int Val1 = 64234123;
            int Val2 = 7512356;
            PropertyInfo idInfo = proto.GetType().GetProperty("Id");
            idInfo.SetValue(proto, RandomId);
            PropertyInfo prop = proto.GetType().GetProperty("Value");
            if ((prop) != null)
            {
                prop.SetValue(proto, Val1);
                _service.Add(proto);
                prop.SetValue(proto, Val2);
                var res = _service.Update(proto);
                var Updated = _service.GetById(RandomId);
                var UpdatedValue = (int)prop.GetValue(Updated);
                Assert.IsTrue(res && (UpdatedValue==Val2));
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

    }
    
    public class AlarmServiceDeviceTests_Device_Tests : AlarmServiceDeviceTests<sconnDevice> { }
    public class AlarmServiceDeviceTests_Zone_Tests : AlarmServiceDeviceTests<sconnAlarmZone> { }
    public class AlarmServiceDeviceTests_User_Tests : AlarmServiceDeviceTests<sconnUser> { }
    public class AlarmServiceDeviceTests_AuthD_Tests : AlarmServiceDeviceTests<sconnAuthorizedDevice> { }
    public class AlarmServiceDeviceTests_GSM_Tests : AlarmServiceDeviceTests<sconnGsmRcpt> { }
    public class AlarmServiceDeviceTests_Input_Tests : AlarmServiceDeviceTests<sconnInput> { }
    public class AlarmServiceDeviceTests_Output_Tests : AlarmServiceDeviceTests<sconnOutput> { }
    public class AlarmServiceDeviceTests_Relay_Tests : AlarmServiceDeviceTests<sconnRelay> { }
    

}
