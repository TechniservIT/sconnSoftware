using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using sconnRem.Navigation;

namespace sconnRem.Tests.Bootstrap
{
    [TestFixture]
    public class AlarmsystemConfigBootstrap
    {
        [Test]
        public void TestBootstrap_Loads_Configuration_Wnd()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion,  new Uri(@"")));
            regionManagerMock.VerifyAll();
        }

        [Test]
        public void TestBootstrap_Loads_Config_DevAuth()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Auth_View)).Verifiable();
            regionManagerMock.VerifyAll();
        }

        [Test]
        public void TestBootstrap_Loads_Config_Gsm()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Gsm_View )).Verifiable();
            regionManagerMock.VerifyAll();
        }

        [Test]
        public void TestBootstrap_Loads_Config_Global()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Global_View )).Verifiable();
            regionManagerMock.VerifyAll();
        }

        [Test]
        public void TestBootstrap_Loads_Config_Zones()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Zone_View)).Verifiable();
            regionManagerMock.VerifyAll();
        }


        [Test]
        public void TestBootstrap_Loads_Config_Users()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Users_View)).Verifiable();
            regionManagerMock.VerifyAll();
        }


    }
}
