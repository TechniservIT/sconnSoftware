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
using sconnConnector.POCO.Config.sconn;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;
using NLog;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.POCO.Config;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmZoneConfigViewModel : GenericAlarmConfigViewModel
    {

        private ObservableCollection<sconnAlarmZone> _config;
        public ObservableCollection<sconnAlarmZone> Config
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

        private ZoneConfigurationService _provider;
        private AlarmSystemConfigManager _manager;

        public ICommand ConfigureZoneCommand { get; set; }
        public ICommand EntitySelected;
        public ICommand ConfigureEntityCommand;

        public void OpenEntityEditContext(sconnAlarmZone device)
        {
            try
            {
                NavigationParameters parameters = new NavigationParameters();
                if (device != null)
                {
                    parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
                    parameters.Add(AlarmSystemEntityListContractNames.Alarm_Contract_Entity_Zone_Edit_Context_Key_Name, device.Id);
                }
                else
                {
                    parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
                }

                GlobalNavigationContext.NavigateRegionToContractWithParam(
                    GlobalViewRegionNames.RNavigationRegion,
                    GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmZoneEditListItemContext,
                    parameters
                    );
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
            
        }

        public void GoToEntityEditView()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(AlarmRegionNames.AlarmConfig_Contract_Zone_Config_View_Key_Name, SelectedIndex);
            NavigateToAlarmContractWithParam(AlarmRegionNames.AlarmConfig_Contract_ZoneConfigView, parameters);
        }

        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnAlarmZone>(_provider.GetAll());
                SelectedIndex = 0; //reset on refresh
                if (Config.Count == 0)
                {
                    Application.Current.Dispatcher.Invoke(() => { OpenEntityEditContext(null); }); //open empty edit context
                }
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

        public void EditZone(sconnAlarmZone zone)
        {
            
        }

        public AlarmZoneConfigViewModel()
        {
            _name = "Zones";
            this._provider = new ZoneConfigurationService(_manager);
        }


        public void SetupCmd()
        {
            ConfigureZoneCommand =   new DelegateCommand<sconnAlarmZone>(EditZone);
        }

        [ImportingConstructor]
        public AlarmZoneConfigViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnAlarmZone>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new ZoneConfigurationService(_manager);
            this._regionManager = regionManager;
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/strefy1.png"; }
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
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_ZoneConfigView))
            {
                return true;    //singleton
            }
            return false;
        }


    }

}
