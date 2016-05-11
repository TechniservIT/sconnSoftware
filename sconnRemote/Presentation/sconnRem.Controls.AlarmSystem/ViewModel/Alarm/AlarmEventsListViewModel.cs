using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService.Device;
using AlarmSystemManagmentService.Event;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{


    [Export]
    public class AlarmEventsListViewModel : GenericAlarmConfigViewModel
    {
        protected EventsService _provider;

        private ObservableCollection<sconnEvent> _config;
        public ObservableCollection<sconnEvent> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }


        public ICommand ShowDeviceStatusCommand { get; set; }
        public ICommand ShowDeviceConfigCommand { get; set; }

        public ICommand ConfigureInputCommand { get; set; }
        public ICommand ConfigureOutputCommand { get; set; }
        public ICommand ConfigureRelayCommand { get; set; }



        private string GetDeviceTypeStatusViewContractNameForDevice(sconnDevice device)
        {
            if (device.Type == sconnDeviceType.Graphical_Keypad)
            {
                return AlarmRegionNames.AlarmStatus_Contract_Device_Keypad_View;
            }
            else if (device.Type == sconnDeviceType.Gsm_Module)
            {
                return AlarmRegionNames.AlarmConfig_Contract_GsmConfigView;
            }
            else if (device.Type == sconnDeviceType.Motherboard)
            {
                return AlarmRegionNames.AlarmStatus_Contract_Device_Motherboard_View;
            }
            else if (device.Type == sconnDeviceType.Pir_Sensor)
            {
                return AlarmRegionNames.AlarmStatus_Contract_Device_Sensor_View;
            }
            else if (device.Type == sconnDeviceType.Relay_Module)
            {
                return AlarmRegionNames.AlarmStatus_Contract_Device_RelayModule_View;
            }
            return null;
        }



        private string GetDeviceTypeConfigureViewContractNameForDevice(sconnDevice device)
        {
            if (device.Type == sconnDeviceType.Graphical_Keypad)
            {
                return AlarmRegionNames.AlarmConfig_Contract_Device_Keypad_View;
            }
            else if (device.Type == sconnDeviceType.Gsm_Module)
            {
                return AlarmRegionNames.AlarmConfig_Contract_Device_Motherboard_View;
            }
            else if (device.Type == sconnDeviceType.Motherboard)
            {
                return AlarmRegionNames.AlarmConfig_Contract_Device_Motherboard_View;
            }
            else if (device.Type == sconnDeviceType.Pir_Sensor)
            {
                return AlarmRegionNames.AlarmConfig_Contract_Device_Sensor_View;
            }
            else if (device.Type == sconnDeviceType.Relay_Module)
            {
                return AlarmRegionNames.AlarmConfig_Contract_Device_RelayModule_View;
            }
            return null;
        }



        private void ShowDevice(sconnDevice device)
        {
            AlarmSystemContext.contextDevice = device;
            SiteNavigationManager.ActivateDeviceContext(device);
            NavigateToAlarmContract(GetDeviceTypeStatusViewContractNameForDevice(device));
        }

      

        private void ConfigureDevice(sconnDevice device)
        {
            AlarmSystemContext.contextDevice = device;
            SiteNavigationManager.ActivateDeviceContext(device);
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_InputsView);
        }


        private void SetupCmds()
        {
            ShowDeviceStatusCommand = new DelegateCommand<sconnDevice>(ShowDevice);
            ShowDeviceConfigCommand = new DelegateCommand<sconnDevice>(ConfigureDevice);
            
        }


        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnEvent>(_provider.GetAll());  //_provider.GetAll().AsQueryable();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {

        }

        public AlarmEventsListViewModel()
        {
            SetupCmds();
            Config = new ObservableCollection<sconnEvent>(new List<sconnEvent>());
            _name = "Gcfg";
            this._provider = new EventsService();
        }


        [ImportingConstructor]
        public AlarmEventsListViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            Config = new ObservableCollection<sconnEvent>(new List<sconnEvent>());
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new EventsService(_manager);
            _regionManager = regionManager;
            this.PropertyChanged += new PropertyChangedEventHandler(OnNotifiedOfPropertyChanged);
        }

        private void OnNotifiedOfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && !String.Equals(e.PropertyName, "IsChanged", StringComparison.Ordinal))
            {
                this.IsChanged = true;
            }
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }


        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_EventsView))
            {
                return true;    //singleton
            }
            return false;
        }

    }


}
