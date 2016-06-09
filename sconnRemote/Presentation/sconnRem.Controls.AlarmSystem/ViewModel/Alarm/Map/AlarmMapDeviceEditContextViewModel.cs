using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
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
    public class AlarmMapDeviceEditContextViewModel : GenericAlarmConfigViewModel   // : AlarmMapEntityEditContextViewModel
    {

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

        private readonly DeviceConfigService _provider;
        private readonly AlarmSystemConfigManager _manager;

      

        public ICommand ConfigureZoneCommand { get; set; }

        public override void GetData()
        {
            try
            {
                Config = (_provider.Get());

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

        }

        public void ShowDevicesMap()
        {


        }
        public AlarmMapDeviceEditContextViewModel()
        {
            _name = "Zones";
            this._provider = new DeviceConfigService(_manager,DeviceId);
        }


        public  void SetupCmd()
        {
            MapContextZonesSelectedCommand = new DelegateCommand(ShowZonesMap);
            MapContextDevicesSelectedCommand = new DelegateCommand(ShowDevicesMap);
            MapContextEntityEditSaveCommand = new DelegateCommand(SaveData);
            ConfigureZoneCommand = new DelegateCommand<sconnAlarmZone>(EditZone);
        }

        [ImportingConstructor]
        public AlarmMapDeviceEditContextViewModel(IRegionManager regionManager)
        {
            Config = new sconnDevice();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new DeviceConfigService(_manager,DeviceId);
            this._regionManager = regionManager;
        }

        public string DisplayedImagePath => "pack://application:,,,/images/strefy1.png";
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                DeviceId = (int)navigationContext.Parameters[AlarmSystemMapContractNames.Alarm_Contract_Map_Device_Edit_Context_Key_Name];
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

                //navigate context  

                //NavigationParameters parameters = new NavigationParameters();
                //parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                //GlobalNavigationContext.NavigateRegionToContractWithParam(
                //   GlobalViewRegionNames.RNavigationRegion,
                //    GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmDeviceEditMapContext,
                //    parameters
                //    );

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
