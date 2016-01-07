using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using iotDash.Controllers.domain.site.Abstract;
using iotDash.Controllers.domain.site.AlarmSystem;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using Moq;
using NUnit.Framework;

namespace webInterfaceTests
{
    [TestFixture]
    public class AlarmSystemControllerTests
    {
        private HttpContextBase _context;
        private AlarmSystemControllerBase controller;

        [SetUp]
        public void Init()
        {
            _context = WebSessionFactory.GetFakeAuthorizedContext();

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var lc = new AlarmSystemViewController(context.Object);
            lc.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            this.controller = lc;
        }

        private void MockTest()
        {
        //    ViewResult viewResult = (ViewResult)controller.ActionInvoker; 
         //   Assert.True(viewResult.ViewName == "ViewForAuthenticatedRequest");
        }

        [Test]
        public void Test_Controller_Alarm_Get()
        {
         //   _controllerContext.Controller = new AlarmSystemViewController(_context);
            
        }

    }


    [TestFixture]
    public abstract class AlarmSystemConfigurationControllerTests<T> where T : new()
    {
        protected IEditableController _controller;

        protected AlarmSystemConfigurationControllerTests()
        {
            _controller = new T() as IEditableController;
        }

        [Test]
        public void Test_Alarm_Controller_Configuration_Search()
        {
            var res = this._controller.Search(Guid.NewGuid().ToString());
            Assert.True(res != null);
        }

        [Test]
        public void Test_Alarm_Controller_Configuration_Remove()
        {
            var res = this._controller.Remove(0);
            Assert.True(res != null);
        }

        [Test]
        public void Test_Alarm_Controller_Configuration_Edit()
        {
            var res = this._controller.Edit(0);
            Assert.True(res != null);
        }

        [Test]
        public void Test_Alarm_Controller_Configuration_Add()
        {
            var res = this._controller.Add();
            Assert.True(res != null);
        }

    }

    [TestFixture]
    public abstract class AlarmSystemControllerTests<T> where T : new()
    {
        public AlarmSystemControllerTests()
        {
            var service =  new T() as AlarmSystemControllerBase;
        }



    }

    public class Controller_AlarmSystemAuthorizedDevicesController_Tests : AlarmSystemConfigurationControllerTests<AlarmSystemAuthorizedDevicesController> { }
    public class Controller_AlarmSystemConfigurationController_Tests : AlarmSystemConfigurationControllerTests<AlarmSystemConfigurationController> { }
    public class Controller_AlarmSystemGsmConfigController_Tests : AlarmSystemConfigurationControllerTests<AlarmSystemGsmConfigController> { }
    public class Controller_AlarmSystemUsersConfigController_Tests : AlarmSystemConfigurationControllerTests<AlarmSystemUsersConfigController> { }
    public class Controller_AlarmSystemZonesConfigController_Tests : AlarmSystemConfigurationControllerTests<AlarmSystemZonesConfigController> { }




}
