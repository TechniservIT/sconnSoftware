using AlarmSystemManagmentService;
using Prism.Mvvm;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmGlobalConfigViewModel : GenericAsyncConfigViewModel
    {
        public sconnGlobalConfig Config { get; set; }
        private GlobalConfigService _provider;
        private AlarmSystemConfigManager _manager;
        

        public override void GetData()
        {
            try
            {
                Config = _provider.Get();

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
            _provider.Update(Config);
        }

        public AlarmGlobalConfigViewModel()
        {
            _name = "Gcfg";
            this._provider = new GlobalConfigService(_manager);
        }

      

        [ImportingConstructor]
        public AlarmGlobalConfigViewModel(IRegionManager regionManager)
        {
            Config = new sconnGlobalConfig();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new GlobalConfigService(_manager);
            this._regionManager = regionManager;
        }


        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (
                navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_Global_View) ||
                navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_NetworkView) ||
                navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_PowerView)  

                )
            {
                return true;    //singleton
            }
            return false;
        }
        

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }

    }

}
