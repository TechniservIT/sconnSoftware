using AlarmSystemManagmentService;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using System.Windows.Input;
using sconnConnector.POCO.Config.sconn;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using NLog;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmAuthConfigViewModel : GenericAsyncConfigViewModel
    {
        public ObservableCollection<sconnAuthorizedDevice> _config { get; set; }
        public ObservableCollection<sconnAuthorizedDevice> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        private AuthorizedDevicesConfigurationService _provider;
        public AlarmSystemConfigManager Manager { get; set; }
        
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


        public ICommand EntitySelected;
        public ICommand ConfigureEntityCommand;

        public void OpenEntityEditContext(sconnAuthorizedDevice device)
        {
            if (device == null || Config.Count <= 0) return;
            NavigationParameters parameters = new NavigationParameters
            {
                {GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID},
                {AlarmSystemEntityListContractNames.Alarm_Contract_Entity_AuthorizedDevice_Edit_Context_Key_Name, device.Id}
            };

            GlobalNavigationContext.NavigateRegionToContractWithParam(
                GlobalViewRegionNames.RNavigationRegion,
                GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmAuthorizedDeviceEditListItemContext,
                parameters
                );
        }

        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnAuthorizedDevice>(_provider.GetAll());
                SelectedIndex = 0; //reset on refresh
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex,ex.Message);
            }
        }

        public  override  void SaveData()
        {
            _provider.SaveChanges();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }

        public void ConfigureEntitySelected(sconnAuthorizedDevice entity)
        {
            
        }

        private void SetupCmds()
        {
            EntitySelected = new DelegateCommand<sconnAuthorizedDevice>(OpenEntityEditContext);
            ConfigureEntityCommand = new DelegateCommand<sconnAuthorizedDevice>(ConfigureEntitySelected);
        }

        public AlarmAuthConfigViewModel()
        {
            _name = "Auth";
            SetupCmds();
            Config = new ObservableCollection<sconnAuthorizedDevice>();
            this._provider = new AuthorizedDevicesConfigurationService(Manager);
        }
        

        [ImportingConstructor]
        public AlarmAuthConfigViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnAuthorizedDevice>();
            SetupCmds();
            this.Manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new AuthorizedDevicesConfigurationService(this.Manager);
            this._regionManager = regionManager;
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
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_AuthConfigView))
            {
                return true;    //singleton
            }
            return false;
        }


    }
}
