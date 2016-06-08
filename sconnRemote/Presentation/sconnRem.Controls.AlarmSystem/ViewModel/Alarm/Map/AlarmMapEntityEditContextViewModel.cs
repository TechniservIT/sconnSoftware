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

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmMapEntityEditContextViewModel : GenericAsyncConfigViewModel
    {

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
        public AlarmMapEntityEditContextViewModel(IRegionManager regionManager)
        {
        
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new ZoneConfigurationService(_manager);
            this._regionManager = regionManager;
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
