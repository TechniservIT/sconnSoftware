using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmMapDeviceEditContextViewModel : GenericAlarmConfigViewModel
    {
        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        public ICommand MapContextZonesSelectedCommand { get; set; }
        public ICommand MapContextDevicesSelectedCommand { get; set; }
        public ICommand MapContextEntityEditSaveCommand { get; set; }

        private sconnDevice _config;
        public sconnDevice Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }
        
        public int DeviceId { get; set; }
        public string DeviceUuid { get; set; }

        private readonly AlarmDevicesConfigService _provider;
        private readonly AlarmSystemConfigManager _manager;


        /***** Zone select *********/

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

        private ZoneConfigurationService ZoneProvider;
        public ICommand ConfigureZoneCommand { get; set; }

        public override void GetData()
        {
            try
            {
                Config = (_provider.GetById(DeviceId));
                Zones = new ObservableCollection<sconnAlarmZone>(ZoneProvider.GetAll());

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

        public void EditZone(sconnAlarmZone zone)
        {

        }


        public void ShowZonesMap()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                GlobalNavigationContext.NavigateRegionToContractWithParam(
                   GlobalViewRegionNames.MainGridContentRegion,
                   AlarmRegionNames.AlarmConfig_Contract_ZoneMapConfigView,
                    parameters
                    );
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        public void ShowDevicesMap()
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                GlobalNavigationContext.NavigateRegionToContractWithParam(
                   GlobalViewRegionNames.MainGridContentRegion,
                   AlarmRegionNames.AlarmConfig_Contract_DeviceMapConfigView,
                    parameters
                    );
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        public AlarmMapDeviceEditContextViewModel()
        {
            _name = "Zones";
            SetupCmd();
            this._provider = new AlarmDevicesConfigService(_manager);
            ZoneProvider = new ZoneConfigurationService(_manager);
        }


        public  void SetupCmd()
        {
            MapContextZonesSelectedCommand = new DelegateCommand(ShowZonesMap);
            MapContextDevicesSelectedCommand = new DelegateCommand(ShowDevicesMap);
            MapContextEntityEditSaveCommand = new DelegateCommand(SaveData);
            ConfigureZoneCommand = new DelegateCommand<sconnAlarmZone>(EditZone);
        }

        [ImportingConstructor]
        public AlarmMapDeviceEditContextViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            Config = new sconnDevice();
            SetupCmd();
            this.AlarmNavService = NavService;
            this._manager = AlarmNavService.alarmSystemConfigManager;
            this._provider = new AlarmDevicesConfigService(_manager);
            ZoneProvider = new ZoneConfigurationService(_manager);
            this._regionManager = regionManager;
        }

        public string DisplayedImagePath => "pack://application:,,,/images/strefy1.png";
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                DeviceId = int.Parse((string)navigationContext.Parameters[AlarmSystemMapContractNames.Alarm_Contract_Map_Device_Edit_Context_Key_Name]);
                this.navigationJournal = navigationContext.NavigationService.Journal;

                BackgroundWorker bgWorker = new BackgroundWorker();
                bgWorker.DoWork += (s, e) => {
                    GetData();
                };
                bgWorker.RunWorkerCompleted += (s, e) =>
                {

                    Loading = false;
                };

                Loading = true;
                bgWorker.RunWorkerAsync();
                

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }


        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var targetsiteUuid = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
            if (targetsiteUuid != siteUUID)
            {
                return true;
            }
            return false;
        }


    }


}
