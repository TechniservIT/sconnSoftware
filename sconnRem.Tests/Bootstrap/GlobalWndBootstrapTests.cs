using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using sconnConnector.Config;
using sconnRem.Navigation;
using sconnRem.View.Menu.GridNavSideMenu;
using sconnRem.View.Menu.SiteNavSideMenu;
using sconnRem.View.Menu.ToolTopMenu;
using sconnRem.Wnd.Main;

namespace sconnRem.Tests.Bootstrap
{
    [TestFixture]
    public class GlobalWndBootstrapTests
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


        

    }
}
