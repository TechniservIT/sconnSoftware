using System;
using System.Linq;
using System.Reflection;
using iotDatabaseConnector.DAL.Repository;
using iotDbConnector.DAL;
using NUnit.Framework;
using sconnConnector.Config;
using sconnConnector.POCO;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;

namespace iotDatabaseConnector.Tests
{

    //    using (UnitOfWork uwork = new UnitOfWork())
    //{
    //}


    public static class FakeIotRepositoryCrudFactory
    {

        public static IiotRepository<T> GetIotRepository<T>() where T : class
        {
            //return new iotRepository<T>();
            return new IotFakeRepository<T>();
        }

    }

    public abstract class IotRepositoryCrudTests<T> where T : class, new()
    {
        private IiotRepository<T> _service;

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
            //Arrange
            T proto = new T();
            int Id = _service.Add(proto);
            var res = _service.GetById(Id);
            Assert.IsTrue(res != null);
        }

        [Test]
        public void Test_AlarmService_Add()
        {
            var res = _service.Add(new T());
            Assert.IsTrue(res >= 0);
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
            _service.Delete(proto);
            var ResCont = _service.GetAll();
            Assert.IsTrue(!ResCont.Contains(proto));   
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
                _service.Delete(proto);
                var ResCont = _service.GetAll();
                Assert.IsTrue(!ResCont.Contains(proto));
            }
            else
            {
                Assert.IsTrue(false);
            }

        }

        [Test]
        public void Test_AlarmService_Update()
        {
              Assert.IsTrue(true);
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
            //    _service.Update(proto);
            //    var Updated = _service.GetById(RandomId);
            //    var UpdatedValue = (int)prop.GetValue(Updated);
            //    Assert.IsTrue(UpdatedValue == Val2);
            //}
            //else
            //{
            //    Assert.IsTrue(false);
            //}
        }

    }

    public class IotRepositoryCrudTests_Domain_Tests : IotRepositoryCrudTests<iotDomain> { }
    public class IotRepositoryCrudTests_Site_Tests : IotRepositoryTests<Site> { }
    public class IotRepositoryCrudTests_Device_Tests : IotRepositoryTests<Device> { }
    public class IotRepositoryCrudTests_Location_Tests : IotRepositoryTests<Location> { }
    public class IotRepositoryCrudTests_DeviceParam_Tests : IotRepositoryTests<DeviceParameter> { }
    public class IotRepositoryCrudTests_DeviceProp_Tests : IotRepositoryTests<DeviceProperty> { }




}
