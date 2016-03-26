using System;
using System.Collections.Generic;
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
using NLog;
using sconnRem.Wnd.Config;

namespace sconnRem.View.Config.AlarmSystem.Comm
{
    /// <summary>
    /// Interaction logic for CommConfigView.xaml
    /// </summary>
    /// 

    [Export]
    [ViewSortHint("05")]
    public partial class CommConfigViewNavigationItem : UserControl, IPartImportsSatisfiedNotification
    {
        private const string mainContentRegionName = "MainContentRegion";
        private Logger nlogger = LogManager.GetCurrentClassLogger();

        // todo: 17a - ContactsView Avatar Option
        // This navigation uri provides additional query data to indicate the 'Avatar' view should be shown.
        private static Uri CommConfigViewUri = new Uri("CommConfigView", UriKind.Relative);

        [Import]
        public IRegionManager regionManager;


        public CommConfigViewNavigationItem()
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
        }

        private void NavigateToContactAvatarsRadioButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Nav_Button_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, CommConfigViewUri
                ,
                (NavigationResult nr) =>
                {
                    var error = nr.Error;
                    var result = nr.Result;
                    if (error != null)
                    {
                        nlogger.Error(error);
                    }
                });
        }



    }

}
