using AlarmSystemManagmentService;
using iotServiceTester.DeviceTypes.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config;
using NUnit.Framework;

namespace iotServiceTester.DeviceTypes.Alarm
{
    
    public class AlarmServiceTests
    {
        
        public void Test_Alarm_Service_As_Device_Service()
        {
            //DeviceConfigService service = new DeviceConfigService();
            //AlarmServiceTestGeneric<sconnDevice> servTest = new AlarmServiceTestGeneric<sconnDevice>(service);
            //servTest.Test_All();
        }
        
        public void Test_Alarm_Service_As_Zone_Service()
        {
            //ZoneConfigurationService service = new ZoneConfigurationService();
            //AlarmServiceTestGeneric<sconnAlarmZone> servTest = new AlarmServiceTestGeneric<sconnAlarmZone>(service);
            //servTest.Test_All();
        }


    }





    [TestFixture(typeof(DeviceConfigService))]
    [TestFixture(typeof(ZoneConfigurationService))]
    public class AlarmServiceTestGeneric<T>  where T : IAlarmSystemConfigurationService<T>, new()
    {
        private IAlarmSystemConfigurationService<T>  _service;

        public AlarmServiceTestGeneric(IAlarmSystemConfigurationService<T> service)
        {
            _service = service;
        }

        public AlarmServiceTestGeneric()
        {
            _service = new T();
        }
        
        [Test]
        public void Test_AlarmService_GetAll()
        {
            Assert.IsTrue(false);
        }

        [Test]
        public void Test_AlarmService_GetById()
        {
            Assert.IsTrue(false);
        }

        [Test]
        public void Test_AlarmService_Add()
        {
            Assert.IsTrue(false);

        }

        [Test]
        public void Test_AlarmService_Edit()
        {

            Assert.IsTrue(false);
        }

        [Test]
        public void Test_AlarmService_Remove()
        {
            Assert.IsTrue(false);

        }

        [Test]
        public void Test_AlarmService_RemoveById()
        {
            Assert.IsTrue(false);

        }
        
    }


}
