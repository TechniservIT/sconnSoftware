using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmMapEntityEditContextViewModel : GenericAlarmConfigViewModel
    {
        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        private readonly ZoneConfigurationService _provider;
        private readonly AlarmSystemConfigManager _manager;

        public ICommand MapContextZonesSelectedCommand { get; set; }
        public ICommand MapContextDevicesSelectedCommand { get; set; }
        public ICommand MapContextEntityEditSaveCommand { get; set; }

        public override void GetData()
        {
           
        }

        public override void SaveData()
        {
         
        }
        
        public AlarmMapEntityEditContextViewModel()
        {
            _name = "Zones";
            this._provider = new ZoneConfigurationService(_manager);
        }

        public void ShowZonesMap()
        {
            
        }

        public void ShowDevicesMap()
        {
            
        }
        

        public virtual void SetupCmd()
        {
            MapContextZonesSelectedCommand = new DelegateCommand(ShowZonesMap);
            MapContextDevicesSelectedCommand = new DelegateCommand(ShowDevicesMap);
            MapContextEntityEditSaveCommand = new DelegateCommand(SaveData);
        }

        [ImportingConstructor]
        public AlarmMapEntityEditContextViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            AlarmNavService = NavService;
            this._manager = NavService.alarmSystemConfigManager;
            this._provider = new ZoneConfigurationService(_manager);
            this._regionManager = regionManager;
        }

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
                    GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmZoneEditMapContext,
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
