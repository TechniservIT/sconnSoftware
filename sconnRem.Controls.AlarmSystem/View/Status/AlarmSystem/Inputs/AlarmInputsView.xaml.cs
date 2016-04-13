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
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.View.Status.Inputs
{
    

    [Export(AlarmRegionNames.AlarmStatus_Contract_InputsView)]
    [ViewSortHint("01")]
    public partial class AlarmInputsView : UserControl, IPartImportsSatisfiedNotification
    {
        private const string MainContentRegionName = GlobalViewRegionNames.MainGridContentRegion;   //GlobalViewRegionNames.MainGridContentRegion
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static Uri configureUri = new Uri("InputsConfig", UriKind.Relative);

        [Import]
        public IRegionManager RegionManager;

        //public AlarmInputsView()
        //{
        //    InitializeComponent();
        //}

        [ImportingConstructor]
        public AlarmInputsView(AlarmSharedDeviceConfigViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }


        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this.RegionManager.Regions[MainContentRegionName];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }

        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {

        }

        private void Configure_Button_Click(object sender, RoutedEventArgs e)
        {
            this.RegionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, configureUri
                ,
                (NavigationResult nr) =>
                {
                    var error = nr.Error;
                    var result = nr.Result;
                    if (error != null)
                    {
                        _nlogger.Error(error);
                    }
                });
        }

    }



}
