using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Prism.Regions;
using sconnConnector.Config;
using sconnRem.Navigation;
using sconnRem.View.Config;
using sconnRem.View.Config.AlarmSystem.Auth;
using sconnRem.View.Config.AlarmSystem.Comm;
using sconnRem.View.Config.AlarmSystem.Global;
using sconnRem.View.Config.AlarmSystem.Gsm;
using sconnRem.View.Config.AlarmSystem.Zone;
using sconnRem.View.Menu.GridNavSideMenu;
using sconnRem.View.Menu.SiteNavSideMenu;
using sconnRem.View.Menu.ToolTopMenu;
using sconnRem.Wnd.Config;
using sconnRem.Wnd.Main;

namespace sconnRem.Tests.Bootstrap
{

    [TestFixture]
    public class AlarmSystemConfigWndBootstrapTests
    {

        IUnityContainer container = new UnityContainer();

        [OneTimeSetUp]
        public void TestBootstrap_Start()
        {
            container.RegisterType<AlarmSystemConfigManager, AlarmSystemConfigManager>();
        }

 

        /**************** Alarm System View Export ****************/
        
        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_View_AlarmSystem_Conf_Auth()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(AuthConfigView));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_View_AlarmSystem_Conf_Comm()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(CommConfigView));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_View_AlarmSystem_Conf_Global()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(GlobalConfigView));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_View_AlarmSystem_Conf_Gsm()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(GsmConfigView));
            Assert.True(exported != null);
        }

        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_View_AlarmSystem_Conf_Zones()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(ZoneConfigView));
            Assert.True(exported != null);
        }



        /************** Alarm System Module Export **************/
        
        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_Module_AlarmSystem_Conf_Auth()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(AlarmAuthConfigModule));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_Module_AlarmSystem_Conf_Comm()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(AlarmCommConfigModule));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_Module_AlarmSystem_Conf_Global()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(GlobalConfigModule));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_Module_AlarmSystem_Conf_Gsm()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(GsmConfigModule));
            Assert.True(exported != null);
        }

        [Test]
        [RequiresSTA]
        public void TestBootstrap_Loads_Module_AlarmSystem_Conf_Zones()
        {
            ConfigNavBootstrapper wnd = new ConfigNavBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(ZoneConfigModule));
            Assert.True(exported != null);
        }






        //[Test]
        //public void TestBootstrap_Loads_Config_DevAuth()
        //{

        //    Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
        //    regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Auth_View)).Verifiable();
        //    regionManagerMock.VerifyAll();
        //}


        //[Test]
        //public void TestBootstrap_Should_Register_View_DevAuth()
        //{
        //    WndGlobalShell wnd = new WndGlobalShell();

        //    Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
        //    regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Auth_View)).Verifiable();
        //    regionManagerMock.VerifyAll();
        //}


        //[Test]
        //public void TestBootstrap_Loads_Config_Gsm()
        //{
        //    Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
        //    regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Gsm_View )).Verifiable();
        //    regionManagerMock.VerifyAll();
        //}

        //[Test]
        //public void TestBootstrap_Loads_Config_Global()
        //{
        //    Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
        //    regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Global_View )).Verifiable();
        //    regionManagerMock.VerifyAll();
        //}

        //[Test]
        //public void TestBootstrap_Loads_Config_Zones()
        //{
        //    Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
        //    regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Zone_View)).Verifiable();
        //    regionManagerMock.VerifyAll();
        //}


        //[Test]
        //public void TestBootstrap_Loads_Config_Users()
        //{
        //    Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
        //    regionManagerMock.Setup(x => x.RequestNavigate(AlarmRegionNames.MainContentRegion, AlarmRegionNames.AlarmUri_Config_Users_View)).Verifiable();
        //    regionManagerMock.VerifyAll();
        //}


    }
}
