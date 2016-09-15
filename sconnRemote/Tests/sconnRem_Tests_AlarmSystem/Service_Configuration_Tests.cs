using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.AlarmSystemUsers;
using AlarmSystemManagmentService.Device;
using AlarmSystemManagmentService.Event;
using NUnit.Framework;
using sconnConnector.Config;
using sconnConnector.POCO;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config.sconn.User;
using sconnConnector.POCO.Device;

namespace sconnRem_Tests_AlarmSystem
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
                    if (prop.PropertyType.GetInterfaces().Contains(typeof(IFakeAbleConfiguration))) //typeof(IFakeAbleConfiguration).IsAssignableFrom(typeof(prop.)))  //fakeAblePropType
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
                service = new AlarmDevicesConfigService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnAlarmZone))
            {
                service = new ZoneConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnAlarmSystemUser))
            {
                service = new AlarmSystemUsersConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnGsmRcpt))
            {
                service = new GsmConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnAuthorizedDevice))
            {
                service = new AuthorizedDevicesConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnEvent))
            {
                service = new EventsService(man) as IAlarmSystemConfigurationService<T>;
            }
            else if (typeof(T) == typeof(sconnRemoteUser))
            {
                service = new UsersConfigurationService(man) as IAlarmSystemConfigurationService<T>;
            }
            else
            {
                service = null;
            }
            service.Online = false; //Disable online config sync for testing
            return service;
        }
    }


    public abstract class AlarmServiceDeviceTests<T> where T : new()
    {
        private IAlarmSystemConfigurationService<T> _service;

        [SetUp]
        public void SetUp()
        {
            _service = FakeAlarmServiceFactory.GetAlarmService<T>();
        }

        [Test]
        public void Test_AlarmService_GetAll()
        {
            SetUp();
            _service.Add(new T());
            var res = _service.GetAll();
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_GetById()
        {
            SetUp();
            _service.Add(new T());
            var res = _service.GetById(0);
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_Add()
        {
            SetUp();
            //TODO - only selected entities
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
            SetUp();
            T proto = new T();
            _service.Add(proto);
            var res = _service.Remove(proto);
            var ResCont = _service.GetAll();
            Assert.IsTrue(res && !ResCont.Contains(proto));
        }

        [Test]
        public void Test_AlarmService_RemoveById()
        {
            SetUp();
            T proto = new T();
            Random r = new Random();
            ushort RandomId = (ushort)r.Next(ushort.MaxValue);
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
            SetUp();
            T proto = new T();
            Random r = new Random();
            ushort RandomId = (ushort)r.Next(ushort.MaxValue);
            ushort Val1 = (ushort)r.Next(ushort.MaxValue);
            ushort Val2 = (ushort)r.Next(ushort.MaxValue);
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
                ushort UpdatedValue = (ushort)prop.GetValue(Updated);
                Assert.IsTrue(res && (UpdatedValue == Val2));
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

    }


    //TODO - readonly
    // public class AlarmServiceDeviceTests_Events_Tests : AlarmServiceDeviceTests<sconnEvent> { }
    //   public class AlarmServiceDeviceTests_Device_Tests : AlarmServiceDeviceTests<sconnDevice> { }
    
    public class AlarmServiceDeviceTests_Zone_Tests : AlarmServiceDeviceTests<sconnAlarmZone> { }
    public class AlarmServiceDeviceTests_SystemUser_Tests : AlarmServiceDeviceTests<sconnAlarmSystemUser> { }
    public class AlarmServiceDeviceTests_RemoteUser_Tests : AlarmServiceDeviceTests<sconnRemoteUser> { }
    public class AlarmServiceDeviceTests_AuthD_Tests : AlarmServiceDeviceTests<sconnAuthorizedDevice> { }
    public class AlarmServiceDeviceTests_GSM_Tests : AlarmServiceDeviceTests<sconnGsmRcpt> { }
    
    public class Service_Configuration_Tests
    {

    }
}
