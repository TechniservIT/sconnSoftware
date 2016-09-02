using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using sconnConnector.POCO.Config.Abstract.Auth;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmRemoteUsersConfigViewModel : GenericAlarmConfigViewModel
    {

        private ObservableCollection<sconnRemoteUser> _config;
        public ObservableCollection<sconnRemoteUser> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                if (_selectedIndex < Config.Count)
                {
                    Application.Current.Dispatcher.Invoke(() => { OpenEntityEditContext(Config[_selectedIndex]); });
                }
            }
        }

        private UsersConfigurationService _provider;
        private AlarmSystemConfigManager _manager;
        public ICommand EntitySelected;
        public ICommand ConfigureEntityCommand;

        public void OpenEntityEditContext(sconnRemoteUser device)
        {
            NavigationParameters parameters = new NavigationParameters();
            if (device != null)
            {
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
                parameters.Add(AlarmSystemEntityListContractNames.Alarm_Contract_Entity_RemoteUser_Edit_Context_Key_Name, device.Id);
            }
            else
            {
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
            }

            GlobalNavigationContext.NavigateRegionToContractWithParam(
                GlobalViewRegionNames.RNavigationRegion,
                GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmRemoteUserEditListItemContext,
                parameters
                );
        }

        public override void GetData()
        {
            try
            {
                if (SiteNavigationManager.Online)
                {
                    Config = new ObservableCollection<sconnRemoteUser>(_provider.GetAll());
                }
                else
                {
                    Config = new ObservableCollection<sconnRemoteUser>(SiteNavigationManager.alarmSystemConfigManager.Config.RemoteUserConfig.Users);
                }
                SelectedIndex = 0; //reset on refresh
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
            foreach (var item in Config)
            {
                _provider.Update(item);
            }
        }

        public AlarmRemoteUsersConfigViewModel()
        {
            _name = "Users";
            this._provider = new UsersConfigurationService(_manager);
        }
        

        [ImportingConstructor]
        public AlarmRemoteUsersConfigViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnRemoteUser>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new UsersConfigurationService(_manager);
            this._regionManager = regionManager;
        }
        
        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/user.png"; }
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
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
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_UsersConfigView))
            {
                return true;    //singleton
            }
            return false;
        }


    }

}
