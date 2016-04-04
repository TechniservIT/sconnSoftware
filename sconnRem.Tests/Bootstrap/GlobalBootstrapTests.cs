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
    public class GlobalBootstrapTests
    {

        [Test]
        public void TestBootstrap_Loads_Navigation_Top()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(GlobalViewRegionNames.RopNavigationRegion, new Uri(@"")));

            //var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);
            //viewModel.OpenMessageCommand.Execute(email);

            regionManagerMock.VerifyAll();
        }

        [Test]
        public void TestBootstrap_Loads_Navigation_Site()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(GlobalViewRegionNames.LNavigationRegion, new Uri(@"")));

            regionManagerMock.VerifyAll();
        }


        [Test]
        public void TestBootstrap_Loads_Navigation_Toolbox()
        {
            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.RequestNavigate(GlobalViewRegionNames.RNavigationRegion, new Uri(@"")));

            regionManagerMock.VerifyAll();
        }


    }
}
