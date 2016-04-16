using iotDash.Controllers.domain.navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotServiceSecurityTester
{
    [TestClass]
    public class webUserAccessTests
    {

        [TestMethod]
        public void Test_Security_Domain_Access()
        {
            // Create controller

            //var controller = new HomeController();
            
            // Check what happens for authenticated user

            //controller.ControllerContext = new FakeControllerContext(controller, "Stephen");

            //var result = controller.Secret() as ActionResult;

            //Assert.IsInstanceOfType(result, typeof(ViewResult));

            //ViewDataDictionary viewData = ((ViewResult)result).ViewData;

            //Assert.AreEqual("Stephen", viewData["userName"]);



            //// Check what happens for anonymous user

            //controller.ControllerContext = new FakeControllerContext(controller);

            //result = controller.Secret() as ActionResult;

            //Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

        }
    }
}
