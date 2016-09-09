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
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.EntityList
{
   
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmEntityInputListItemEditViewModel : GenericAlarmConfigViewModel
    {
        private IAlarmSystemNavigationService AlarmNavService { get; set; }
        public ICommand EntitySaveCommand { get; set; }
        public sconnDevice Device { get; set; }

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
        
        private readonly AlarmDevicesConfigService _provider;
        public int InputId { get; set; }
        public int DeviceId { get; set; }

        public string DisplayedImagePath => "pack://application:,,,/images/strefy1.png";

        public ICommand ConfigureZoneCommand { get; set; }

        public override void GetData()
        {
            try
            {

                if (AlarmNavService.Online)
                {
                    Device = (_provider.GetById(DeviceId));
                    Config = Device.Inputs.FirstOrDefault(z => z.Id == InputId);
                }
                else
                {
                    Device = AlarmNavService.alarmSystemConfigManager.Config.DeviceConfig.Devices.FirstOrDefault(d => d.Id == DeviceId);
                    Config = Device.Inputs.FirstOrDefault(z => z.Id == InputId);
                }

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
            _provider.Update(Device);
        }
        

        public AlarmEntityInputListItemEditViewModel()
        {
            _name = "Zones";
            SetupCmd();
            this._provider = new AlarmDevicesConfigService(_manager);
        }


        public void AddEntity()
        {
            try
            {

                //TODO - error or direct to dev authorization adding - no manual entry

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        public void RemoveEntity()
        {
            try
            {
                //TODO - error or direct to dev authorization adding - no manual entry
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

    
        public void SetupCmd()
        {
            EntitySaveCommand = new DelegateCommand(SaveData);
        }

        [ImportingConstructor]
        public AlarmEntityInputListItemEditViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            AlarmNavService = NavService;
            Config = new sconnInput();
            SetupCmd();
            this._manager = AlarmNavService.alarmSystemConfigManager;
            this._provider = new AlarmDevicesConfigService(_manager);
            this._regionManager = regionManager;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                this.navigationJournal = navigationContext.NavigationService.Journal;
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                int sentInputId = 0;
                int sentDeviceId = 0;
                bool gotInputId = int.TryParse((string)navigationContext.Parameters[AlarmSystemEntityListContractNames.Alarm_Contract_Entity_Input_Edit_Context_Key_Name], out sentInputId);    
                bool gotDevId = int.TryParse((string)navigationContext.Parameters[AlarmSystemEntityListContractNames.Alarm_Contract_Entity_Input_Device__Edit_Context_Key_Name], out sentDeviceId);
                if (gotInputId && gotDevId)
                {
                    InputId = sentInputId;
                    DeviceId = sentDeviceId;
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
