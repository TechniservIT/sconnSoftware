using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ZoneConfigurationService _provider;
        private AlarmSystemConfigManager _manager;

        public ICommand ConfigureZoneCommand { get; set; }

        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnAlarmZone>(_provider.GetAll());

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
