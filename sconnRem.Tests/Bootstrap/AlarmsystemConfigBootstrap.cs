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
using sconnRem.View.Menu.GridNavSideMenu;
using sconnRem.View.Menu.SiteNavSideMenu;
using sconnRem.View.Menu.ToolTopMenu;
using sconnRem.Wnd.Config;
using sconnRem.Wnd.Main;

namespace sconnRem.Tests.Bootstrap
{

    [TestFixture]
    public class AlarmsystemConfigBootstrap
    {

        IUnityContainer container = new UnityContainer();

        [OneTimeSetUp]
        public void TestBootstrap_Start()
        {
            container.RegisterType<AlarmSystemConfigManager, AlarmSystemConfigManager>();
        }

        /************** View export ***************/


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Should_Register_View_TopNavigation()
        {
            GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(ToolTopMenuView));
            Assert.True(exported != null);
        }



        [Test]
        [RequiresSTA]
        public void TestBootstrap_Should_Register_View_SiteNavigation()
        {
            GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(SiteNavSideMenuView));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Should_Register_View_ToolbarNavigation()
        {
            GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(GridNavSideMenuView));
            Assert.True(exported != null);
        }


        /***********  Module export ***************/


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Should_Register_Module_ToolbarNavigation()
        {
            GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(GridNavSideMenuModule));
            Assert.True(exported != null);
        }

        [Test]
        [RequiresSTA]
        public void TestBootstrap_Should_Register_Module_SiteNavigation()
        {
            GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(SiteNavSideMenuModule));
            Assert.True(exported != null);
        }


        [Test]
        [RequiresSTA]
        public void TestBootstrap_Should_Register_Module_TopNavigation()
        {
            GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
            wnd.Run();
            var container = wnd.GetContainer();
            var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(ToolTopMenuModule));
            Assert.True(exported != null);
        }




        //[Test]
        //[RequiresSTA]
        //public void TestBootstrap_Loads_Configuration_Wnd()
        //{
        //    GlobalWndBootstrapper wnd = new GlobalWndBootstrapper();
        //    wnd.Run();
        //    var container = wnd.GetContainer();
        //    // container.Catalog.Parts.ElementType
        //    var exported = container.Catalog.Parts.Where(x => x.GetType() == typeof(CommConfigView));
        //    //GetExportedValues<CommConfigView>(AlarmRegionNames.AlarmConfig_Contract_CommConfigView);
        //    Assert.True(exported != null);

        //    //Mock<WndGlobalShell> regionManagerMock = new Mock<WndGlobalShell>();
        //    //regionManagerMock.s
        //    //  wnd.RegionManager.Regions.First().Views.Contains()
        //    //Mock<WndConfigureSiteShell> wndMock = new Mock<WndConfigureSiteShell>();
        //    //wndMock.Setup(x => x.);
        //    //regionManagerMock.VerifyAll();
        //}

            

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
