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

        public bool Online
        {
            get
            {
                return SiteNavigationManager.Online;
            }

            set
            {
                SiteNavigationManager.Online = value;
                OnPropertyChanged();
            }
        }

        private void ToggleConnectivityMode()
        {
            SiteNavigationManager.OpenSiteWizard();
        }

        private void SetupCmds()
        {
            ToggleConnectivityModeCommand = new DelegateCommand(ToggleConnectivityMode);
        }

        public ConnectivityModeFooterViewModel()
        {
            SetupCmds();
        }

        [ImportingConstructor]
        public ConnectivityModeFooterViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            this._regionManager = regionManager;
        }
        
    }


}
