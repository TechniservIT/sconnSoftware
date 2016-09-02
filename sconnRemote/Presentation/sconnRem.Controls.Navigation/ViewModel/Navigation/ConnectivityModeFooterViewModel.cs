using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.Config.Storage;
using sconnConnector.POCO.Config;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.Wnd.Tools;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{

    [Export]
    public class ConnectivityModeFooterViewModel : BindableBase
    {
        public sconnSite Site { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        public ICommand ToggleConnectivityModeCommand { get; set; }

        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        public bool Online
        {
            get
            {
                return AlarmNavService.Online;
            }

            set
            {
                AlarmNavService.Online = value;
                OnPropertyChanged();
            }
        }

        private void ToggleConnectivityMode()
        {
        //    SiteNavigationManager.OpenSiteWizard();
        }

        private void SetupCmds()
        {
            ToggleConnectivityModeCommand = new DelegateCommand(ToggleConnectivityMode);
        }
        

        [ImportingConstructor]
        public ConnectivityModeFooterViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            AlarmNavService = NavService;
            SetupCmds();
            this._regionManager = regionManager;
            AlarmNavService.PropertyChanged += AlarmNavService_PropertyChanged;
        }

        private void AlarmNavService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Online");
        }
    }


}
