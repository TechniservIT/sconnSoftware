using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnPrismSharedContext;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmInputConfigViewModel : GenericAsyncConfigViewModel
    {
        public sconnInput Config { get; set; }
        private DeviceConfigService _provider;
        private AlarmSystemConfigManager _manager;
        

        public ICommand NavigateBackCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }

        private void GetData()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }
        
        public AlarmInputConfigViewModel()
        {
            this._manager = AlarmSystemContext.GetManager();
            _name = "Dev";
            this._provider = new DeviceConfigService(_manager);
        }

        public void NavigateBack()
        {
            try
            {
                this.navigationJournal?.GoBack();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            SiteNavigationManager.SaveInput(this.Config);
        }

        [ImportingConstructor]
        public AlarmInputConfigViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            this._manager = SiteNavigationManager.alarmSystemConfigManager; // (AlarmSystemConfigManager)manager;
            this._regionManager = regionManager;
            //this._provider = new DeviceConfigService(SiteNavigationManager.alarmSystemConfigManager, SiteNavigationManager.CurrentContextDevice.DeviceId); 
            GetData();
        }

        private void SetupCmds()
        {
            NavigateBackCommand = new DelegateCommand(NavigateBack);
            SaveConfigCommand = new DelegateCommand(SaveData);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            string inputId = (string)navigationContext.Parameters[AlarmRegionNames.AlarmConfig_Contract_Input_Config_View_Key_Name];
            if (inputId != null)
            {
               this.Config = SiteNavigationManager.InputForId(inputId);
            }

            this.navigationJournal = navigationContext.NavigationService.Journal;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (this.Config == null)
            {
                return true;
            }

            var inputId = navigationContext.Parameters[AlarmRegionNames.AlarmConfig_Contract_Input_Config_View_Key_Name]; // GetRequestedEmailId(navigationContext);
            return inputId.Equals(Config.UUID);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }



}
