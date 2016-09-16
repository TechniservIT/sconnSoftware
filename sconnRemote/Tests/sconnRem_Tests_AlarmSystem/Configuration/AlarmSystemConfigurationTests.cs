using System;
using System.Reflection;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.AlarmSystemUsers;
using AlarmSystemManagmentService.Device;
using AlarmSystemManagmentService.Event;
using NUnit.Framework;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config.sconn.User;
using sconnRem_Tests_AlarmSystem;

namespace sconnRemote_AlarmSystemModelsTests
{
   
    [TestFixture]
    public abstract class AlarmEntitySerializationTests<T> where T : IAlarmSystemGenericConfigurationEntity, new()
    {
        private IAlarmSystemGenericConfigurationEntity _entity;

        [SetUp]
        public void SetUp()
        {
           _entity = new T();
           _entity.Fake();
        }

        [Test]
        public void Test_Entity_Serialization()
        {
            SetUp();
            IAlarmSystemGenericConfigurationEntity before = new T();
            before = _entity.DeepClone();
            byte[] serialized = _entity.Serialize();
            IAlarmSystemGenericConfigurationEntity after = new T();
            after.Deserialize(serialized);
            after.UUID = before.UUID;
            Assert.IsTrue(before.Equals(after));
        }
        
    }

    [TestFixture]
    public class AlarmEntitySerializationTests_Device_Tests : AlarmEntitySerializationTests<sconnDevice> { }

    [TestFixture]
    public class AlarmEntitySerializationTests_Zone_Tests : AlarmEntitySerializationTests<sconnAlarmZone> { }

    [TestFixture]
    public class AlarmEntitySerializationTests_SystemUser_Tests : AlarmEntitySerializationTests<sconnAlarmSystemUser> { }

    [TestFixture]
    public class AlarmEntitySerializationTests_RemoteUser_Tests : AlarmEntitySerializationTests<sconnRemoteUser> { }
    public class AlarmEntitySerializationTests_Events_Tests : AlarmEntitySerializationTests<sconnEvent> { }
    public class AlarmEntitySerializationTests_AuthD_Tests : AlarmEntitySerializationTests<sconnAuthorizedDevice> { }
    public class AlarmEntitySerializationTests_GSM_Tests : AlarmEntitySerializationTests<sconnGsmRcpt> { }


}
