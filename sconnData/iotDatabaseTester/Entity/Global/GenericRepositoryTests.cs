using iotDash.Controllers.domain.navigation;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseTester.Entity
{

    [TestClass]
    public class GenericRepositoryTests
    {
        private IIotContextBase context;

        public GenericRepositoryTests(IIotContextBase cont)
        {
            this.context = cont;
        }


        /******************************** Domain ***************************/


        /***************** Device *******************/



        /***************** Site *******************/


        /***************** Actions *******************/


        /***************** Properties *******************/


        /************ Location ***************/


        [TestMethod]
        public void Test_Repository_Location_Create()
        {
            Assert.IsTrue(true);
            var mockContext = new Mock<IIotContextBase>();
          //  mockContext.Setup(c =>c.HttpContext.Response.Redirect("~/Some/Other/Place"));
            var controller = new HomeController();
            //controller.ControllerContext = mockContext.Object;
            //controller.SendMeSomewhereElse();
            mockContext.Verify();
        }

        [TestMethod]
        public void Test_Repository_Location_Remove()
        {

        }

        [TestMethod]
        public void Test_Repository_Location_Get_By_Id()
        {

        }



    }

}
