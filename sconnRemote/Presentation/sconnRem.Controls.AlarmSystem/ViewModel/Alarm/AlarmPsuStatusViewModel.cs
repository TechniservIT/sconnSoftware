using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    public class AlarmPsuStatusViewModel : GenericAsyncConfigViewModel
    {
        public sconnGlobalConfig Config { get; set; }
        private GlobalConfigService _provider;
        private AlarmSystemConfigManager _manager;


        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

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

        public AlarmPsuStatusViewModel()
        {
            _name = "Gcfg";
            this._provider = new GlobalConfigService(_manager);
        }



        [ImportingConstructor]
        public AlarmPsuStatusViewModel(IRegionManager regionManager)
        {
            Config = new sconnGlobalConfig();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new GlobalConfigService(_manager);
            this._regionManager = regionManager;
            GetData();
        }


        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.Equals(AlarmRegionNames.AlarmStatus_Contract_Global_View))
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
