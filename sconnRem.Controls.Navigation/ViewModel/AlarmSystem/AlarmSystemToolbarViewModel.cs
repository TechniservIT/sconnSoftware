﻿using System;
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
        public ICommand Show_Alarm_Gsm_Command { get; set; }
        public ICommand Show_Alarm_Users_Command { get; set; }



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
            SiteNavigationManager.ShowConfigureScreen();
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



        private void SetupCmds()
        {
            Show_Configure_Command = new DelegateCommand<sconnSite>(ShowConfigure);

            Show_Alarm_Inputs_Command = new DelegateCommand<sconnSite>(ShowInputs);
            Show_Alarm_Outputs_Command = new DelegateCommand<sconnSite>(ShowOutputs);
            Show_Alarm_Relay_Command = new DelegateCommand<sconnSite>(ShowRelays);

            //Show_Alarm_Zones_Command = new DelegateCommand<sconnSite>(ShowConfigure);
            //Show_Alarm_Users_Command = new DelegateCommand<sconnSite>(ShowConfigure);

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
