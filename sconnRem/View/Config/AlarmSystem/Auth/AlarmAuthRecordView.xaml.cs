using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sconnRem.View.Config.AlarmSystem.Auth
{


    [Export]
    [ViewSortHint("04")]
    public partial class AlarmAuthRecordView : UserControl, IPartImportsSatisfiedNotification
    {
        private const string mainContentRegionName = "MainContentRegion";

        // todo: 17a - ContactsView Avatar Option
        // This navigation uri provides additional query data to indicate the 'Avatar' view should be shown.
        private static Uri contactsAvatarsViewUri = new Uri("ContactsView?Show=Avatars", UriKind.Relative);

        [Import]
        public IRegionManager regionManager;

        public AlarmAuthRecordView()
        {
            InitializeComponent();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this.regionManager.Regions[mainContentRegionName];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }

        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {
            this.UpdateNavigationButtonState(e.Uri);
        }

        private void UpdateNavigationButtonState(Uri uri)
        {
         //   this.NavigateToContactAvatarsRadioButton.IsChecked = (uri == contactsAvatarsViewUri);
        }

        private void NavigateToContactAvatarsRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(mainContentRegionName, contactsAvatarsViewUri);
        }
    }

}
