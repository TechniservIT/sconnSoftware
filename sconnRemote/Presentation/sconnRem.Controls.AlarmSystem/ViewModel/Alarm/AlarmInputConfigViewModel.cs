using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmInputConfigViewModel : GenericAlarmConfigViewModel
    {

        private ZoneConfigurationService _provider;
        private AlarmSystemConfigManager _manager;
        public ICommand NavigateBackCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }

        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        private ObservableCollection<sconnAlarmZone> _zones;
        public ObservableCollection<sconnAlarmZone> Zones
        {
            get { return _zones; }
            set
            {
                _zones = value;
                OnPropertyChanged();
            }
        }
        
        private sconnInput _config;
        public sconnInput Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }


        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                if (_selectedIndex < Zones.Count)
                {
                  //  Application.Current.Dispatcher.Invoke(() => { OpenEntityEditContext(Config[_selectedIndex]); });
                }
            }
        }


        private void GetData()
        {
            try
            {
                if (AlarmNavService.Online)
                {
                    Zones = new ObservableCollection<sconnAlarmZone>(_provider.GetAll());
                }
                else
                {
                    Zones = new ObservableCollection<sconnAlarmZone>(AlarmNavService.alarmSystemConfigManager.Config.ZoneConfig.Zones);
                }
               
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }
        
        public AlarmInputConfigViewModel()
        {
            this._manager = AlarmNavService.alarmSystemConfigManager;
            _name = "Dev";
            this._provider = new ZoneConfigurationService(_manager);
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

        public IRegionNavigationJournal navigationJournal { get; set; }

        private void SaveData()
        {
            AlarmNavService.SaveInput(this.Config);
        }

        [ImportingConstructor]
        public AlarmInputConfigViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            SetupCmds();
            this._manager = AlarmNavService.alarmSystemConfigManager;
            AlarmNavService = NavService;
            this._regionManager = regionManager;
            this._provider = new ZoneConfigurationService(AlarmNavService.alarmSystemConfigManager); 
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
            string inputId = (string)navigationContext.Parameters[AlarmRegionNames.AlarmConfig_Contract_Input_Config_View_Key_Name];
            if (inputId != null)
            {
               this.Config = AlarmNavService.InputForId(inputId);
            }

            this.navigationJournal = navigationContext.NavigationService.Journal;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (this.Config == null)
            {
                return true;
            }
            var targetsiteUuid = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
            if (targetsiteUuid != siteUUID)
            {
                var inputId = navigationContext.Parameters[AlarmRegionNames.AlarmConfig_Contract_Input_Config_View_Key_Name]; // GetRequestedEmailId(navigationContext);
                return inputId.Equals(Config.UUID);
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }



}
