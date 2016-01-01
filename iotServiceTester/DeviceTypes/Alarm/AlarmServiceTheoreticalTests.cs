using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using sconnConnector.POCO.Config.sconn;
using Xunit;

namespace iotServiceTester.DeviceTypes.Alarm
{
    
    
    public abstract  class AlarmServiceTheoreticalTests<T>
    {
        [Fact]
        public void Test_AlarmService_GetAll<T>()
        {
            IAlarmSystemConfigurationService<T> _service;
            _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            var res = _service.GetAll();
            Assert.True(res != null);
        }

        [Fact]
        public void Test_AlarmService_GetById<T>()
        {
            IAlarmSystemConfigurationService<T> _service;
            _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            var res = _service.GetById(0);
            Assert.True(res != null);
        }

        [Fact]
        public void Test_AlarmService_Add<T>() where T : new()
        {
            IAlarmSystemConfigurationService<T> _service;
            _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            var res = _service.Add(new T());
            Assert.True(res);
        }

        [Fact]
        public void Test_AlarmService_Edit<T>()
        {

        }

        [Fact]
        public void Test_AlarmService_Remove<T>() where T: new ()
        {
            IAlarmSystemConfigurationService<T> _service;
            _service = FakeAlarmServiceFactory.GetAlarmService<T>();
            T proto = new T();
            _service.Add(proto);
            var res = _service.Remove(proto);
            Assert.True(res);
        }

        [Fact]
        public void Test_AlarmService_RemoveById<T>()
        {

        }
    }


    public class AlarmServiceTheoretical_Device_Tests : AlarmServiceTheoreticalTests<sconnDevice> { }
    public class AlarmServiceTheoretical_Zone_Tests : AlarmServiceTheoreticalTests<sconnAlarmZone> { }

}
