using AlarmSystemManagmentService;
using iotServiceTester.DeviceTypes.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
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
    
        public static AlarmSystemConfigManager GetAlarmConfigManager()
        {
            Device dev = FakeAlarmServiceFactory.GetAlarmDevice();
            AlarmSystemConfigManager man = new AlarmSystemConfigManager(dev.EndpInfo, dev.Credentials);
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
            Assert.IsTrue(res);
        }

        [Test]
        public void Test_AlarmService_RemoveById()
        {

        }
    }


    public class AlarmServiceDeviceTests_Device_Tests : AlarmServiceDeviceTests<sconnDevice> { }
    public class AlarmServiceDeviceTests_Zone_Tests : AlarmServiceDeviceTests<sconnAlarmZone> { }
    public class AlarmServiceDeviceTests_User_Tests : AlarmServiceDeviceTests<sconnUser> { }
    public class AlarmServiceDeviceTests_AuthD_Tests : AlarmServiceDeviceTests<sconnAuthorizedDevices> { }
    public class AlarmServiceDeviceTests_GSM_Tests : AlarmServiceDeviceTests<sconnGsmConfig> { }


    //[TestClass()]
    //public class AlarmServiceGenericTests<T> where T : new()
    //{
    //    private IAlarmSystemConfigurationService<T> _service;

    //    public AlarmServiceGenericTests()
    //    {
    //        _service = FakeAlarmServiceFactory.GetAlarmService<T>();
    //    }

    //    public void AlarmServiceGenericTests_Devices<T>() where T: sconnDevice
    //    {
    //        _service = FakeAlarmServiceFactory.GetAlarmService<T>();
    //    }

    //    [TestMethod()]
    //    public void Test_AlarmService_GetAll()
    //    {
    //        var res = _service.GetAll();
    //        Assert.IsTrue(res != null);
    //    }

    //    [TestMethod()]
    //    public void Test_AlarmService_GetById()
    //    {
    //        var res = _service.GetById(0);
    //        Assert.IsTrue(res != null);
    //    }

    //    [TestMethod()]
    //    public void Test_AlarmService_Add()
    //    {
    //        var res = _service.Add(new T());
    //        Assert.IsTrue(res);
    //    }

    //    [TestMethod()]
    //    public void Test_AlarmService_Edit()
    //    {

    //    }

    //    [TestMethod()]
    //    public void Test_AlarmService_Remove()
    //    {
    //        T proto = new T();
    //        _service.Add(proto);
    //        var res = _service.Remove(proto);
    //        Assert.IsTrue(res);
    //    }

    //    [TestMethod()]
    //    public void Test_AlarmService_RemoveById()
    //    {

    //    }
    //}

}
