using System;
using System.Linq;
using System.Reflection;
using iotDbConnector.DAL;
using NUnit.Framework;
using sconnConnector.Config;
using sconnConnector.POCO;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;

namespace iotDatabaseConnector.Tests
{


    public static class FakeIotRepositoryFactory
    {

        public static iotRepository<T> GetIotRepository<T>() where T : class
        {
            return new iotRepository<T>();
        }

    }

    public abstract class IotRepositoryTests<T> where T : class, new()
    {
        private iotRepository<T> _service;

        [SetUp]
        public void SetUp()
        {
            _service = FakeIotRepositoryFactory.GetIotRepository<T>();
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
            Assert.IsTrue(res > 0);
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
           // var res = _service.Delete(proto);
            var ResCont = _service.GetAll();
            Assert.IsTrue(false);   //res && !ResCont.Contains(proto));
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
               // var res = service.Delete(proto);    //RemoveById(RandomId);
                var ResCont = _service.GetAll();
                Assert.IsTrue(false);   //res && !ResCont.Contains(proto));
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
            //    var res = _service.Update(proto);
                var Updated = _service.GetById(RandomId);
                var UpdatedValue = (int)prop.GetValue(Updated);
                Assert.IsTrue(false);   //res && (UpdatedValue == Val2));
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

    }

    public class IotRepositoryTests_Domain_Tests : IotRepositoryTests<iotDomain> { }
    public class IotRepositoryTests_Site_Tests : IotRepositoryTests<Site> { }
    public class IotRepositoryTests_Device_Tests : IotRepositoryTests<Device> { }
    public class IotRepositoryTests_Location_Tests : IotRepositoryTests<Location> { }
    public class IotRepositoryTests_DeviceParam_Tests : IotRepositoryTests<DeviceParameter>    { }
    public class IotRepositoryTests_DeviceProp_Tests : IotRepositoryTests<DeviceProperty> { }



    }
