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

        public static Device GetFakeAlarmDevice()
        {
            Device fakeDevice = new Device();
            DeviceCredentials fakeCredentials = new DeviceCredentials();
            fakeCredentials.Fake();
            fakeDevice.Credentials = fakeCredentials;
            EndpointInfo fakeEndpointInfo = new EndpointInfo();
            fakeEndpointInfo.Fake();
            fakeDevice.EndpInfo = fakeEndpointInfo;
            fakeDevice.Fake();
            return fakeDevice;;
        }


        public static IAlarmSystemConfigurationService<T> GetAlarmService<T>()
        {
            AlarmSystemConfigManager man = FakeAlarmServiceFactory.GetAlarmConfigManager();
            man.RemoteDevice = GetFakeAlarmDevice();

            IAlarmSystemConfigurationService<T> service;
            var type = typeof (T);
            if (typeof (T) == typeof (sconnAlarmZone))
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
            if (service != null)
            {
                service.Online = false; //Disable online config sync for testing
            }

            return service;
        }

        



        public static IAlarmSystemSingleEntityConfigurationService<T> GetAlarmSingleEntityConfigurationService<T>()
        {
            AlarmSystemConfigManager man = FakeAlarmServiceFactory.GetAlarmConfigManager();
            man.RemoteDevice = GetFakeAlarmDevice();

            IAlarmSystemSingleEntityConfigurationService<T> service;
            var type = typeof(T);
            if (type == typeof(sconnDevice))
            {
                service = new DeviceConfigService(man) as IAlarmSystemSingleEntityConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnGlobalConfig))
            {
                service = new GlobalConfigService(man) as IAlarmSystemSingleEntityConfigurationService<T>;
            }
            else
            {
                service = null;
            }
            if (service != null)
            {
                service.Online = false; //Disable online config sync for testing
            }

            return service;
        }

    }


    public abstract class AlarmServiceConfigurationTests<T> where T : new()
    {

        [SetUp]
        public void  SetUp()
        {
         
        }

        [Test]
        public void Test_AlarmService_GetAll()
        {
            var _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            var res = _service.GetAll();
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_GetById()
        {
            var _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            var res = _service.GetById(0);
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_Add() 
        {
            var _service = FakeAlarmServiceFactory.GetAlarmService<T>();
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
            var _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            T proto = new T();
            _service.Add(proto);
            var res = _service.Remove(proto);
            var ResCont = _service.GetAll();
            Assert.IsTrue(res && !ResCont.Contains(proto));
        }

        [Test]
        public void Test_AlarmService_RemoveById()
        {
            var _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            T proto = new T();
            int RandomId = 156164831;
            PropertyInfo prop = proto.GetType().GetProperty("Id");
            if ((prop) != null)
            {
                prop.SetValue(proto, RandomId);
                _service.Add(proto);
                var res = _service.RemoveById(RandomId);
                var resCont = _service.GetAll();
                Assert.IsTrue(res && !resCont.Contains(proto));
            }
            else
            {
                Assert.IsTrue(false);
            }

        }

        [Test]
        public void Test_AlarmService_Update()
        {
            var _service = FakeAlarmServiceFactory.GetAlarmService<T>();
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
    
    public class AlarmServiceDeviceTests_Zone_Tests : AlarmServiceConfigurationTests<sconnAlarmZone> { }
    public class AlarmServiceDeviceTests_User_Tests : AlarmServiceConfigurationTests<sconnUser> { }
    public class AlarmServiceDeviceTests_AuthD_Tests : AlarmServiceConfigurationTests<sconnAuthorizedDevice> { }
    public class AlarmServiceDeviceTests_GSM_Tests : AlarmServiceConfigurationTests<sconnGsmRcpt> { }

    //public class AlarmServiceDeviceTests_Input_Tests : AlarmServiceDeviceTests<sconnInput> { }
    //public class AlarmServiceDeviceTests_Output_Tests : AlarmServiceDeviceTests<sconnOutput> { }
    //public class AlarmServiceDeviceTests_Relay_Tests : AlarmServiceDeviceTests<sconnRelay> { }




    /***********  Single entity *************/


    public class AlarmServiceSingleConfigurationTests_Device_Tests : AlarmServiceSingleConfigurationTests<sconnDevice> { }
    public class AlarmServiceSingleConfigurationTests_GlobalCfg_Tests : AlarmServiceSingleConfigurationTests<sconnGlobalConfig> { }

    public abstract class AlarmServiceSingleConfigurationTests<T> where T : new()
    {
        private IAlarmSystemSingleEntityConfigurationService<T> _service;

        [SetUp]
        public void SetUp()
        {
            _service = FakeAlarmServiceFactory.GetAlarmSingleEntityConfigurationService<T>();
        }
        
        [Test]
        public void Test_AlarmService_Get()
        {

        }
        
        [Test]
        public void Test_AlarmService_Update()
        {
            //T proto = new T();
            //int RandomId = 156164831;   //Test updating by Id property
            //int Val1 = 64234123;
            //int Val2 = 7512356;
            //PropertyInfo idInfo = proto.GetType().GetProperty("Id");
            //idInfo.SetValue(proto, RandomId);
            //PropertyInfo prop = proto.GetType().GetProperty("Value");
            //if ((prop) != null)
            //{
            //    prop.SetValue(proto, Val1);
            //    _service.Add(proto);
            //    prop.SetValue(proto, Val2);
            //    var res = _service.Update(proto);
            //    var Updated = _service.GetById(RandomId);
            //    var UpdatedValue = (int)prop.GetValue(Updated);
            //    Assert.IsTrue(res && (UpdatedValue == Val2));
            //}
            //else
            //{

            //}
            Assert.IsTrue(false);
        }


    }













}
