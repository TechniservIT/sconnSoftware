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
using sconnConnector.POCO.Config.Abstract.Auth;
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
            return sys;
        }

        public static AlarmSystemConfigManager GetAlarmConfigManager()
        {
            Device dev = FakeAlarmServiceFactory.GetAlarmDevice();
            AlarmSystemConfigManager man = new AlarmSystemConfigManager(dev.EndpInfo, dev.Credentials);
            man.Config = FakeAlarmServiceFactory.GetFakeAlarmSconnAlarmSystem();
            return man;
        }

        public static IAlarmSystemConfigurationService<T> GetAlarmService<T>()
        {
            AlarmSystemConfigManager man = FakeAlarmServiceFactory.GetAlarmConfigManager();
            if (typeof(T) == typeof(sconnDevice))
            {
               return new DeviceConfigService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof (T) == typeof (sconnAlarmZone))
            {
                return new ZoneConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnUser))
            {
                return new UsersConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnGsmConfig))
            {
                return new GsmConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnAuthorizedDevices))
            {
                return new AuthorizedDevicesConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else
            {
                return null;
            }
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
            FieldInfo field;
            if ((field = proto.GetType().GetField("Id")) != null)
            {
                field.SetValue(proto, RandomId);
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
            int ChangedId = 64234123;
            FieldInfo field;
            if ((field = proto.GetType().GetField("Id")) != null)
            {
                field.SetValue(proto, RandomId);
                _service.Add(proto);
                field.SetValue(proto, ChangedId);
                var res = _service.Update(proto);
                Assert.IsTrue(res);
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
    public class AlarmServiceDeviceTests_AuthD_Tests : AlarmServiceDeviceTests<sconnAuthorizedDevices> { }
    public class AlarmServiceDeviceTests_GSM_Tests : AlarmServiceDeviceTests<sconnGsmConfig> { }
    public class AlarmServiceDeviceTests_Input_Tests : AlarmServiceDeviceTests<sconnInput> { }
    public class AlarmServiceDeviceTests_Output_Tests : AlarmServiceDeviceTests<sconnOutput> { }
    public class AlarmServiceDeviceTests_Relay_Tests : AlarmServiceDeviceTests<sconnRelay> { }
    

}
