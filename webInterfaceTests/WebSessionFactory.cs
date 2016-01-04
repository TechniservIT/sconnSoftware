using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iotDash.Session;
using Moq;

namespace webInterfaceTests
{
    public static class WebSessionFactory
    {

        public static HttpContextBase GetAuthorizedContext()
        {
            var context = new Mock<HttpContextBase>();
            var mockIdentity = new Mock<IIdentity>();
            context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            mockIdentity.Setup(x => x.Name).Returns("test_name");
            return context.Object;
            // return DomainSession.GetDataContextForUserContext();
        }

        public static HttpContextBase GetFakeAuthorizedContext()
        {
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.IsAuthenticated).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            DomainSession.LoadFakeContextForUserContext(context.Object);

            return context.Object;
        }

        public static ControllerContext GetFakeAuthorizedControllerContext()
        {
            
            // create mock principal
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("admin");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            var _controllerContext = mockContext.Object;
            return _controllerContext;
        }

}


}
