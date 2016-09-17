using System;
using AlarmSystemManagmentService;
using Moq;
using NUnit.Framework;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;
using sconnRem.ViewModel.Alarm;

namespace sconnRemote_NavigationTests
{
    [TestFixture]
    public class sconnRemNavigationTests
    {

    }



    [TestFixture]
    public class sconnRemAlarmSystemNavigationTests
    {


        [Test]
        public void Test_AlarmSys_AuthConfig_WhenGoingBack_ThenNavigatesBack()
        {
            var emailServiceMock = new Mock<AuthorizedDevicesConfigurationService>();

            var viewModel = new AlarmAuthConfigViewModel();

            var journalMock = new Mock<IRegionNavigationJournal>();

            var navigationServiceMock = new Mock<IRegionNavigationService>();
            navigationServiceMock.SetupGet(svc => svc.Journal).Returns(journalMock.Object);

            NavigationContext context = new NavigationContext(navigationServiceMock.Object, new Uri("location", UriKind.Relative));
            context.Parameters.Add("EmailId", Guid.NewGuid());

            ((INavigationAware)viewModel).OnNavigatedTo(context);

            //  viewModel.GoBackCommand.Execute(null);

            journalMock.Verify(j => j.GoBack());

            Assert.IsTrue(true);
        }



        [Test]
        public void Test_AlarmSys_AuthConfig_WhenAskedCanNavigateForSameQuery_ThenReturnsTrue()
        {
            var email = new sconnAuthorizedDevice();

            var emailServiceMock = new Mock<AuthorizedDevicesConfigurationService>();
            //emailServiceMock
            //    .Setup(svc => svc.GetEmailDocument(email.Id))
            //   .Returns(email)
            //   .Verifiable();

            var viewModel = new AlarmAuthConfigViewModel();

            NavigationContext context = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location", UriKind.Relative));
            context.Parameters.Add("EmailId", email.Id);

            ((INavigationAware)viewModel).OnNavigatedTo(context);

            bool canNavigate =
                ((INavigationAware)viewModel).IsNavigationTarget(context);

            Assert.IsTrue(canNavigate);
        }

        [Test]
        public void Test_AlarmSys_AuthConfig_WhenAskedCanNavigateForDifferentQuery_ThenReturnsFalse()
        {
            var email = new sconnAuthorizedDevice();

            var emailServiceMock = new Mock<AuthorizedDevicesConfigurationService>();
            //emailServiceMock
            //    .Setup(svc => svc.GetEmailDocument(email.Id))
            //   .Returns(email)
            //   .Verifiable();

            var viewModel = new AlarmAuthConfigViewModel();

            NavigationContext context = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location", UriKind.Relative));
            context.Parameters.Add("EmailId", email.Id);

            ((INavigationAware)viewModel).OnNavigatedTo(context);

            context = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location", UriKind.Relative));
            context.Parameters.Add("EmailId", new Guid());

            bool canNavigate =
                ((INavigationAware)viewModel).IsNavigationTarget(context);

            Assert.IsFalse(canNavigate);
        }
    }

}
