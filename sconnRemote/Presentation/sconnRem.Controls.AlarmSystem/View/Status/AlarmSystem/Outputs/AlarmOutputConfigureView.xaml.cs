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
using Prism;
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Outputs
{

    [Export(AlarmRegionNames.AlarmConfig_Contract_Output_Config_View)]
    [ViewSortHint("01")]
    public partial class AlarmOutputConfigureView : UserControl, IPartImportsSatisfiedNotification, IActiveAware, INavigationAware
    {
        private const string MainContentRegionName = GlobalViewRegionNames.MainGridContentRegion;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static Uri configureUri = new Uri("InputsConfig", UriKind.Relative);

        [Import]
        public IRegionManager RegionManager;

        [ImportingConstructor]
        public AlarmOutputConfigureView(AlarmSharedDeviceConfigViewModel viewModel)
        {
            viewModel.UpdateActiveIo();
            this.DataContext = viewModel;
            InitializeComponent();
        }

        //public AlarmOutputConfigureView()
        //{
        //    this.DataContext = SiteNavigationManager new AlarmSharedDeviceConfigViewModel();
        //    InitializeComponent();
        //}


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

        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
          
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }
    }



}
