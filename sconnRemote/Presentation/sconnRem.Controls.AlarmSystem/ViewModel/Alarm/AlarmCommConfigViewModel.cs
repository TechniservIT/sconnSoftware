using AlarmSystemManagmentService;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmCommConfigViewModel : GenericAsyncConfigViewModel

    {
        public sconnGlobalConfig CommConfig { get; set; }
        private GlobalConfigService _provider;
        public AlarmSystemConfigManager Manager { get; set; }
        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;
        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        public override void GetData()
        {
            try
            {
                if (AlarmNavService.Online)
                {
                    CommConfig = _provider.Get();
                }
                else
                {
                    CommConfig = AlarmNavService.alarmSystemConfigManager.Config.GlobalConfig;
                }
               
                
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
            _provider.Update(CommConfig);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }

        
        public AlarmCommConfigViewModel()
        {
            _name = "Auth";
            this._provider = new GlobalConfigService(Manager);
        }



        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_CommConfigView))
            {
                return true;    //singleton
            }
            return false;
        }

      

        [ImportingConstructor]
        public AlarmCommConfigViewModel(IRegionManager regionManager)
        {
            this._regionManager = regionManager;

        }

        [ImportingConstructor]
        public AlarmCommConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            CommConfig = new sconnGlobalConfig();
            AlarmNavService = NavService;
            this.Manager = (AlarmSystemConfigManager)manager;
            this._provider = new GlobalConfigService(this.Manager);
            this._regionManager = regionManager;
            GetData();
        }


    }
    
}
