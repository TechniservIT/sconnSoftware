using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
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
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;
using Xceed.Wpf.Toolkit;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    public class AlarmDeviceListViewModel : GenericAlarmConfigViewModel
    {
        private ObservableCollection<sconnDevice> _config;
        private AlarmDevicesConfigService _provider;

        public ObservableCollection<sconnDevice> Config {
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


        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnDevice>(_provider.GetAll());  //_provider.GetAll().AsQueryable();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
         
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
            if (navigationContext.Uri.Equals(AlarmRegionNames.AlarmUri_Status_Device_List_View))
            {
                return true;    //singleton
            }
            return false;
        }

    }


}