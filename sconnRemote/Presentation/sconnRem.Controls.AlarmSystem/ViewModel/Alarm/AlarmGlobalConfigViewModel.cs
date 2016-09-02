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
using Prism.Commands;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmGlobalConfigViewModel : GenericAlarmConfigViewModel
    {

        private sconnGlobalConfig _config;
        public sconnGlobalConfig Config
        {
            get
            {
                return _config;
            }
            set
            {
                _config = value;
                OnPropertyChanged();
            } 
            
        }

        private GlobalConfigService _provider;
        private AlarmSystemConfigManager _manager;

        private string _armedIconPath;
        public string ArmStateIconPath
        {
            get
            {
                if (Config.Armed)
                {
                   return "pack://application:,,,/images/zazb-1000.jpg";
                }
                return "pack://application:,,,/images/rozb-1000.jpg";
            }
            set
            {
                _armedIconPath = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleArmedCommand { get; set; }
        
        public override void GetData()
        {
            try
            {
                if (SiteNavigationManager.Online)
                {
                    Config = _provider.Get();
                }
                else
                {
                    Config = SiteNavigationManager.alarmSystemConfigManager.Config.GlobalConfig;
                }
                
                ArmStateIconPath = new Guid().ToString();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public void ToggleArmed()
        {
            SaveData(); //Upload toggled arm state
            GetData();
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

        public void SetupCmds()
        {
            ToggleArmedCommand = new DelegateCommand(ToggleArmed);
        }
      

        [ImportingConstructor]
        public AlarmGlobalConfigViewModel(IRegionManager regionManager)
        {
            Config = new sconnGlobalConfig();
            SetupCmds();
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
