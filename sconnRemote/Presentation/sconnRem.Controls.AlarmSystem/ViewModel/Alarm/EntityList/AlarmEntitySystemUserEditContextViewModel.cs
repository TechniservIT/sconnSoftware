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
using AlarmSystemManagmentService.AlarmSystemUsers;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config.sconn.User;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.EntityList
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmEntitySystemUserEditContextViewModel : GenericAlarmConfigViewModel
    {

        public ICommand EntityAddCommand { get; set; }
        public ICommand EntityRemoveCommand { get; set; }
        public ICommand EntitySaveCommand { get; set; }

        private sconnAlarmSystemUser _config;
        public sconnAlarmSystemUser Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        public int EntityId { get; set; }
        private readonly AlarmSystemUsersConfigurationService _provider;
        public string DisplayedImagePath => "pack://application:,,,/images/strefy1.png";

        public override void GetData()
        {
            try
            {
                Config = (_provider.GetById(EntityId));

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

        public AlarmEntitySystemUserEditContextViewModel()
        {
            _name = "AuthDev";
            SetupCmd();
            this._provider = new AlarmSystemUsersConfigurationService(_manager);
        }


        public void AddEntity()
        {
            try
            {
                var entities = new ObservableCollection<sconnAlarmSystemUser>(_provider.GetAll());
                sconnAlarmSystemUser d = new sconnAlarmSystemUser();
                d.UUID = Guid.NewGuid().ToString();
                d.Id = entities.Count;
                d.Enabled = false;
                d.Card = Guid.NewGuid().ToString();
                d.Code = Guid.NewGuid().ToString();
                d.Permissions = 0;
                d.DomainId = 0;
                d.AllowedUntil = DateTime.MaxValue;

                bool added = false;
                BackgroundWorker bgWorker = new BackgroundWorker();
                bgWorker.DoWork += (s, e) => {
                    added = _provider.Add(d);
                };
                bgWorker.RunWorkerCompleted += (s, e) =>
                {
                    if (added)
                    {
                        //reload list
                        NavigationParameters parameters = new NavigationParameters
                        {
                            {GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID}
                        };
                        GlobalNavigationContext.NavigateRegionToContractWithParam(
                           GlobalViewRegionNames.MainGridContentRegion,
                           AlarmRegionNames.AlarmConfig_Contract_GsmConfigView,
                            parameters
                            );
                    }
                };

                Loading = true;
                bgWorker.RunWorkerAsync();

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
                var entities = new ObservableCollection<sconnAlarmSystemUser>(_provider.GetAll());
                var toRemove = entities.FirstOrDefault(d => d.Id == EntityId);
                if (toRemove != null)
                {
                    bool removed = false;
                    BackgroundWorker bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += (s, e) => {
                        removed = _provider.Remove(toRemove);
                    };
                    bgWorker.RunWorkerCompleted += (s, e) =>
                    {
                        if (removed)
                        {
                            //reload list
                            NavigationParameters parameters = new NavigationParameters
                        {
                            {GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID}
                        };
                            GlobalNavigationContext.NavigateRegionToContractWithParam(
                               GlobalViewRegionNames.MainGridContentRegion,
                               AlarmRegionNames.AlarmConfig_Contract_GsmConfigView,
                                parameters
                                );
                        }
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


        public void SetupCmd()
        {
            EntityAddCommand = new DelegateCommand(AddEntity);
            EntityRemoveCommand = new DelegateCommand(RemoveEntity);
            EntitySaveCommand = new DelegateCommand(SaveData);
        }

        [ImportingConstructor]
        public AlarmEntitySystemUserEditContextViewModel(IRegionManager regionManager)
        {
            Config = new sconnAlarmSystemUser();
            SetupCmd();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new AlarmSystemUsersConfigurationService(_manager);
            this._regionManager = regionManager;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                EntityId = int.Parse((string)navigationContext.Parameters[AlarmSystemEntityListContractNames.Alarm_Contract_Entity_GsmRcpt_Edit_Context_Key_Name]);
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
            try
            {
                var targetsiteUuid = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                var targetEntityId = int.Parse((string)navigationContext.Parameters[AlarmSystemEntityListContractNames.Alarm_Contract_Entity_GsmRcpt_Edit_Context_Key_Name]);
                if (targetsiteUuid != siteUUID)
                {
                    if (targetEntityId == this.EntityId)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }

}
