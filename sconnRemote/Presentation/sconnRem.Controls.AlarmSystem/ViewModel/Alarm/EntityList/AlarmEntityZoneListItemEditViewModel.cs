﻿using System;
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
using sconnConnector.POCO.Config.sconn;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmEntityZoneListItemEditViewModel : GenericAlarmConfigViewModel
    {

            public ICommand EntityAddCommand { get; set; }
            public ICommand EntityRemoveCommand { get; set; }
            public ICommand EntitySaveCommand { get; set; }

            private sconnAlarmZone _config;
            public sconnAlarmZone Config
            {
                get { return _config; }
                set
                {
                    _config = value;
                    OnPropertyChanged();
                }
            }

            public int ZoneId { get; set; }
            private readonly ZoneConfigurationService _provider;
            public string DisplayedImagePath => "pack://application:,,,/images/strefy1.png";
            public ICommand ConfigureZoneCommand { get; set; }

            public override void GetData()
            {
                try
                {
                    Config = (_provider.GetById(ZoneId));

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

            public AlarmEntityZoneListItemEditViewModel()
            {
                _name = "Zones";
                SetupCmd();
                this._provider = new ZoneConfigurationService(_manager);
            }


            public void AddEntity()
            {
                try
                {

                   
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


                }
                catch (Exception ex)
                {
                    _nlogger.Error(ex, ex.Message);
                }

            }

        public void ShowDevicesMap()
            {
                try
                {
                    NavigationParameters parameters = new NavigationParameters();
                    parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                    GlobalNavigationContext.NavigateRegionToContractWithParam(
                       GlobalViewRegionNames.MainGridContentRegion,
                       AlarmRegionNames.AlarmConfig_Contract_DeviceMapConfigView,
                        parameters
                        );
                }
                catch (Exception ex)
                {
                    _nlogger.Error(ex, ex.Message);
                }
            }

            public void SetupCmd()
            {
                EntityAddCommand = new DelegateCommand(AddEntity);
                EntityRemoveCommand = new DelegateCommand(RemoveEntity);
                EntitySaveCommand = new DelegateCommand(SaveData);
            }

            [ImportingConstructor]
            public AlarmEntityZoneListItemEditViewModel(IRegionManager regionManager)
            {
                Config = new sconnAlarmZone();
                SetupCmd();
                this._manager = SiteNavigationManager.alarmSystemConfigManager;
                this._provider = new ZoneConfigurationService(_manager);
                this._regionManager = regionManager;
            }

            public override void OnNavigatedTo(NavigationContext navigationContext)
            {
                try
                {
                    siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                    ZoneId = int.Parse((string)navigationContext.Parameters[AlarmSystemMapContractNames.Alarm_Contract_Map_Zone_Edit_Context_Key_Name]);
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
