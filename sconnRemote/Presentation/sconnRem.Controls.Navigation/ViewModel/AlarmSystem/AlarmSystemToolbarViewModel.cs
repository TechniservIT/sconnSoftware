using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnPrismGenerics.View.Interface;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.Navigation.ViewModel.AlarmSystem
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmSystemToolbarViewModel : BindableBase, INavigableView
    {
        public sconnSite Site { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private IRegionNavigationJournal navigationJournal;
        public string siteUUID { get; set; }

        public ICommand Show_Status_Command { get; set; }
        public ICommand Show_GlobalStatus_Command { get; set; }
        public ICommand Show_Alarm_Map_Command { get; set; }
         
        public ICommand Show_Alarm_Inputs_Command { get; set; }
        public ICommand Show_Alarm_Outputs_Command { get; set; }
        public ICommand Show_Alarm_Relay_Command { get; set; }
        public ICommand Show_Alarm_Devices_Command { get; set; }

        public ICommand Show_Alarm_Network_Command { get; set; }
        public ICommand Show_Alarm_Events_Command { get; set; }
        public ICommand Show_Alarm_Power_Command { get; set; }

        public ICommand Show_Alarm_HumiditySensors_Command { get; set; }
        public ICommand Show_Alarm_TempratureSensors_Command { get; set; }

        //Full CRUD entities
        public ICommand Show_Alarm_AuthConfig_Command { get; set; }
        public ICommand Show_Alarm_GsmRcpts_Command { get; set; }
        public ICommand Show_Alarm_SystemUsers_Command { get; set; }
        public ICommand Show_Alarm_Zones_Command { get; set; }
        public ICommand Show_Alarm_Users_Command { get; set; }

        
        private void ShowConfigure(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_Global_View);  
        }

        private void NavigateToAlarmContract(string contractName)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                GlobalNavigationContext.NavigateRegionToContractWithParam
                    (
                       GlobalViewRegionNames.MainGridContentRegion,
                        contractName,
                        parameters
                    );
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }
        
        private void ShowInputs(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_InputsView);
        }
        
        private void ShowOutputs(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_OutputsView);
        }

        private void ShowRelays(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_RelaysView);
        }

        private void ShowDevices(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_Device_List_View);
        }


        private void ShowZones(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_ZonesView);  
        }


        private void ShowUsers(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_UsersConfigView);
        }
        
        private void ShowAuthConfig(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_AuthConfigView);
        }

        private void ShowNetworkConfig(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_NetworkView);
        }
        
        private void ShowEventsConfig(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_EventsView);
        }

        private void ShowPowerConfig(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_PowerView);
        }

        private void ShowTemperatureSensorsList(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_TemperatureSensorsView);
        }
        
        private void ShowHumiditySensorsList(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_HumiditySensorsView);
        }

        private void ShowGsmRcptsList(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_GsmRcptConfigView);
        }

        private void ShowSystemUsers(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_SystemUsersConfigView);
        }

        private void ShowSystemMap(sconnSite site)
        {
            NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_ZoneMapConfigView);
        }
        
        private void SetupCmds()
        {
            Show_GlobalStatus_Command = new DelegateCommand<sconnSite>(ShowConfigure);
            Show_Alarm_Map_Command = new DelegateCommand<sconnSite>(ShowSystemMap);

            Show_Alarm_Inputs_Command = new DelegateCommand<sconnSite>(ShowInputs);
            Show_Alarm_Outputs_Command = new DelegateCommand<sconnSite>(ShowOutputs);
            Show_Alarm_Relay_Command = new DelegateCommand<sconnSite>(ShowRelays);

            Show_Alarm_Devices_Command= new DelegateCommand<sconnSite>(ShowDevices);
            
            Show_Alarm_Zones_Command = new DelegateCommand<sconnSite>(ShowZones);
            Show_Alarm_Users_Command = new DelegateCommand<sconnSite>(ShowUsers);
            Show_Alarm_AuthConfig_Command = new DelegateCommand<sconnSite>(ShowAuthConfig);

            Show_Alarm_Network_Command = new DelegateCommand<sconnSite>(ShowNetworkConfig);
            Show_Alarm_Events_Command = new DelegateCommand<sconnSite>(ShowEventsConfig);
            Show_Alarm_Power_Command = new DelegateCommand<sconnSite>(ShowPowerConfig);

            Show_Alarm_HumiditySensors_Command = new DelegateCommand<sconnSite>(ShowHumiditySensorsList);
            Show_Alarm_TempratureSensors_Command = new DelegateCommand<sconnSite>(ShowTemperatureSensorsList);

            Show_Alarm_GsmRcpts_Command = new DelegateCommand<sconnSite>(ShowGsmRcptsList);
            Show_Alarm_SystemUsers_Command = new DelegateCommand<sconnSite>(ShowSystemUsers);
    }

        public AlarmSystemToolbarViewModel()
        {
            SetupCmds();
        }

        public AlarmSystemToolbarViewModel(sconnSite site)
        {
            this.Site = site;
            SetupCmds();
        }


        [ImportingConstructor]
        public AlarmSystemToolbarViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            this._regionManager = regionManager;
        }

        
        public AlarmSystemToolbarViewModel(IRegionManager regionManager, sconnSite site)
        {
            this.Site = site;
            SetupCmds();
            this._regionManager = regionManager;
        }

        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;

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
        
        public  void OnNavigatedTo(NavigationContext navigationContext)
        {
            siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
            if (siteUUID != null)
            {
                this.Site = SiteNavigationManager.CurrentContextSconnSite;
            }
            else
            {
                siteUUID = Guid.NewGuid().ToString();
            }
            this.navigationJournal = navigationContext.NavigationService.Journal;
        }

        public  bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var targetsiteUuid = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
            if (targetsiteUuid != siteUUID)
            {
                return true;
            }
            return false;
        }
        
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
         
        }

        public void AcceptChanges()
        {
         
        }

        public bool IsChanged { get; }
    }


}
