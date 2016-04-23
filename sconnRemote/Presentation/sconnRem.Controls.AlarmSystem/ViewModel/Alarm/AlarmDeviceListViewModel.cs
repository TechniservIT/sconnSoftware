using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using NLog;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    public class AlarmDeviceListViewModel : BindableBase, IActiveAware, INavigationAware, IChangeTracking, INotifyPropertyChanged
    {
        private ObservableCollection<sconnDevice> _config; 
        public ObservableCollection<sconnDevice> Config {
            get { return _config; }
            set
            {
                _config = value;
                this.OnPropertyChanged();
            }

        }

        private AlarmDevicesConfigService _provider;
        private AlarmSystemConfigManager _manager;
        private readonly IRegionManager _regionManager;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        private int ChangeTrack = 0;
        public bool IsChanged { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public ICommand ShowDeviceStatusCommand { get; set; }
        public ICommand ShowDeviceConfigCommand { get; set; }

        public ICommand ConfigureInputCommand { get; set; }
        public ICommand ConfigureOutputCommand { get; set; }
        public ICommand ConfigureRelayCommand { get; set; }
        
        private void NavigateToAlarmContract(string contractName)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, contractName
                    ,
                    (NavigationResult nr) =>
                    {
                        var error = nr.Error;
                        var result = nr.Result;
                        if (error != null)
                        {
                            _nlogger.Error(error);
                        }
                    });
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }


        private void NavigateToAlarmContractWithParam(string contractName,NavigationParameters param)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, new Uri(contractName + param, UriKind.Relative)
                    ,
                    (NavigationResult nr) =>
                    {
                        var error = nr.Error;
                        var result = nr.Result;
                        if (error != null)
                        {
                            _nlogger.Error(error);
                        }
                    });
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }

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

        private void ShowOutputConfigView(sconnOutput input)
        {
            try
            {
                foreach (var device in Config)
                {
                    if (device.Outputs.Contains(input))
                    {
                        SiteNavigationManager.ActivateDeviceContext(device);
                        SiteNavigationManager.ActivateOutputContext(input);
                        NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_Output_Config_View);
                    }
                }
            }
            catch (Exception ex)
            {
                    
            }
        }

        private void ShowRelayConfigView(sconnRelay input)
        {
            try
            {
                foreach (var device in Config)
                {
                    if (device.Relays.Contains(input))
                    {
                        SiteNavigationManager.ActivateDeviceContext(device);
                        SiteNavigationManager.ActivateRelayContext(input);
                        NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_Relay_Config_View);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void ShowInputConfigView(sconnInput input)
        {
            try
            {
                foreach (var device in Config)
                {
                    if (device.Inputs.Contains(input))
                    {
                        SiteNavigationManager.ActivateDeviceContext(device);
                        SiteNavigationManager.ActivateInputContext(input);

                        NavigationParameters parameters = new NavigationParameters();
                        parameters.Add(AlarmRegionNames.AlarmConfig_Contract_Input_Config_View_Key_Name, input.UUID);
                        NavigateToAlarmContractWithParam(AlarmRegionNames.AlarmConfig_Contract_Input_Config_View,parameters);
                     //   NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_Input_Config_View);
                    }
                }
            }
            catch (Exception ex)
            {

            }
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

            ConfigureInputCommand = new DelegateCommand<sconnInput>(ShowInputConfigView);
            ConfigureOutputCommand = new DelegateCommand<sconnOutput>(ShowOutputConfigView);
            ConfigureRelayCommand = new DelegateCommand<sconnRelay>(ShowRelayConfigView);
        }



        private void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnDevice>(_provider.GetAll());  //_provider.GetAll().AsQueryable();
                ChangeTrack++;
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
          //  _provider.Update(Config);
        }

        public AlarmDeviceListViewModel()
        {
            SetupCmds();
            Config = new ObservableCollection<sconnDevice>(new List<sconnDevice>());
            _name = "Gcfg";
            this._provider = new AlarmDevicesConfigService(_manager);
        }

        
        [ImportingConstructor]
        public AlarmDeviceListViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            Config = new ObservableCollection<sconnDevice>(new List<sconnDevice>());
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new AlarmDevicesConfigService(_manager);
            this._regionManager = regionManager;
            this.PropertyChanged += new PropertyChangedEventHandler(OnNotifiedOfPropertyChanged);
            GetData();
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
        

        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetData(); //update list
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;    //singleton
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
         
        }

        public void AcceptChanges()
        {
           
        }

    }


}