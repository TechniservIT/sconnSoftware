using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmMapDeviceEditContextViewModel : AlarmMapEntityEditContextViewModel
    {

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
        private readonly AlarmSystemConfigManager _manager;

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

        public AlarmMapDeviceEditContextViewModel()
        {
            _name = "Zones";
            this._provider = new ZoneConfigurationService(_manager);
        }


        public override void SetupCmd()
        {
            ConfigureZoneCommand = new DelegateCommand<sconnAlarmZone>(EditZone);
        }

        [ImportingConstructor]
        public AlarmMapDeviceEditContextViewModel(IRegionManager regionManager)
        {
            Config = new sconnAlarmZone();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new ZoneConfigurationService(_manager);
            this._regionManager = regionManager;
        }

        public string DisplayedImagePath => "pack://application:,,,/images/strefy1.png";
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                this.navigationJournal = navigationContext.NavigationService.Journal;

                //navigate context  
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                GlobalNavigationContext.NavigateRegionToContractWithParam(
                   GlobalViewRegionNames.RNavigationRegion,
                    GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmDeviceEditMapContext,
                    parameters
                    );
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
