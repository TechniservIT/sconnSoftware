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
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;
using Xceed.Wpf.Toolkit;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmDeviceListViewModel : GenericAlarmConfigViewModel
    {
        private ObservableCollection<sconnDevice> _config;
        private AlarmDevicesConfigService _provider;
        private IAlarmSystemNavigationService AlarmNavService { get; set; }

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

        public ICommand ToggleOutputCommand { get; set; }




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
            AlarmNavService.CurrentContextDevice = device;
            AlarmNavService.ActivateDeviceContext(device);
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
                        AlarmNavService.ActivateDeviceContext(device);
                        AlarmNavService.ActivateOutputContext(input);
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
                        AlarmNavService.ActivateDeviceContext(device);
                        AlarmNavService.ActivateRelayContext(input);
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

                        NavigationParameters parameters = new NavigationParameters();
                        parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                        GlobalNavigationContext.NavigateRegionToContractWithParam(
                            GlobalViewRegionNames.RNavigationRegion,
                            GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmInputEditListItemContext,
                            parameters
                            );

                        //AlarmNavService.ActivateDeviceContext(device);
                        //AlarmNavService.ActivateInputContext(input);

                        //NavigationParameters parameters = new NavigationParameters();
                        //parameters.Add(AlarmRegionNames.AlarmConfig_Contract_Input_Config_View_Key_Name, input.UUID);
                        //NavigateToAlarmContractWithParam(AlarmRegionNames.AlarmConfig_Contract_Input_Config_View, parameters);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ConfigureDevice(sconnDevice device)
        {
            AlarmNavService.CurrentContextDevice = device;
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_InputsView);
        }


        private void SetupCmds()
        {
            ShowDeviceStatusCommand = new DelegateCommand<sconnDevice>(ShowDevice);
            ShowDeviceConfigCommand = new DelegateCommand<sconnDevice>(ConfigureDevice);

            ConfigureInputCommand = new DelegateCommand<sconnInput>(ShowInputConfigView);
            ConfigureOutputCommand = new DelegateCommand<sconnOutput>(ShowOutputConfigView);
            ConfigureRelayCommand = new DelegateCommand<sconnRelay>(ShowRelayConfigView);

            ToggleOutputCommand = new DelegateCommand<sconnOutput>(ToggleOutput);
        }

        private void ToggleOutput(sconnOutput output)
        {
            AlarmNavService.SaveOutputGeneric(output);
            GetData();
        }

        public override void GetData()
        {
            try
            {
                if (AlarmNavService.Online)
                {
                    Config = new ObservableCollection<sconnDevice>(AlarmNavService.GetDevices());
                }
                else
                {
                    Config = new ObservableCollection<sconnDevice>(AlarmNavService.alarmSystemConfigManager.Config.DeviceConfig.Devices);
                }
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
        public AlarmDeviceListViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            SetupCmds();
            AlarmNavService = NavService;
            Config = new ObservableCollection<sconnDevice>(new List<sconnDevice>());
            this._manager = AlarmNavService.alarmSystemConfigManager;
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
            var targetsiteUuid = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
            if (
                    (
                    navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_Device_List_View) ||
                    navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_InputsView) ||
                    navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_OutputsView) ||
                    navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_RelaysView)    ||
                     navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_HumiditySensorsView) ||
                      navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_TemperatureSensorsView)
                      )
                      &&
                      (targetsiteUuid != siteUUID)
                )
            {
                return true;    //singleton
            }
            return false;
        }

    }


}