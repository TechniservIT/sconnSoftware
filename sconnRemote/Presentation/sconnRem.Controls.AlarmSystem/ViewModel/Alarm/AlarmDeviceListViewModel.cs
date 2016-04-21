using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
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
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    public class AlarmDeviceListViewModel : BindableBase
    {
        public List<sconnDevice> Config { get; set; }
        private AlarmDevicesConfigService _provider;
        private AlarmSystemConfigManager _manager;
        private readonly IRegionManager _regionManager;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

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

        private void ShowInputConfigView(sconnInput input)
        {
            try
            {
                foreach (var device in Config)
                {
                    if (device.Inputs.Contains(input))
                    {
                        // device.ActiveInput = input;
                        //device.ActiveInputId = string.Copy(input.UUID);
                        SiteNavigationManager.ActivateDeviceContext(device);
                        SiteNavigationManager.ActivateInputContext(input);
                        NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_Input_Config_View);
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
        }



        private void GetData()
        {
            try
            {
                Config = _provider.GetAll();

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
            Config = new List<sconnDevice>();
            _name = "Gcfg";
            this._provider = new AlarmDevicesConfigService(_manager);
        }

        
        [ImportingConstructor]
        public AlarmDeviceListViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            Config = new List<sconnDevice>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new AlarmDevicesConfigService(_manager);
            this._regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }

    }


}