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
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.Navigation.ViewModel.AlarmSystem
{

    [Export]
    public class AlarmSystemToolbarViewModel : BindableBase
    {
        public sconnSite Site { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public ICommand Show_Status_Command { get; set; }
        public ICommand Show_Configure_Command { get; set; }

        public ICommand Show_Alarm_Inputs_Command { get; set; }
        public ICommand Show_Alarm_Outputs_Command { get; set; }
        public ICommand Show_Alarm_Relay_Command { get; set; }

        public ICommand Show_Alarm_Zones_Command { get; set; }
        public ICommand Show_Alarm_AuthConfig_Command { get; set; }
        public ICommand Show_Alarm_Users_Command { get; set; }

        public ICommand Show_Alarm_Devices_Command { get; set; }

        public ICommand Show_Alarm_Network_Command { get; set; }
        public ICommand Show_Alarm_Events_Command { get; set; }
        public ICommand Show_Alarm_Power_Command { get; set; }


        private void ViewSite(sconnSite site)
        {
            this._regionManager.RequestNavigate(GlobalViewRegionNames.TopContextToolbarRegion, NavContextToolbarRegionNames.ContextToolbar_AlarmSystem_ViewUri
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

        private void ShowConfigure(sconnSite site)
        {
            //  SiteNavigationManager.ShowConfigureScreen();
            NavigateToAlarmContract(AlarmRegionNames.AlarmStatus_Contract_Global_View);  
        }

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

        private void NavigateToAlarmUri(Uri uri)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, uri
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
            NavigateToAlarmContract(AlarmRegionNames.AlarmConfig_Contract_ZoneConfigView);
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


        private void SetupCmds()
        {
            Show_Configure_Command = new DelegateCommand<sconnSite>(ShowConfigure);

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

    }


}
