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
using Prism.Regions;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmZoneConfigViewModel : BindableBase    // ObservableObject, IPageViewModel
    {
        public ObservableCollection<sconnAlarmZone> Config { get; set; }
        private ZoneConfigurationService _provider;
        private AlarmSystemConfigManager _manager;
        private readonly IRegionManager _regionManager;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        private void GetData()
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

        private void SaveData()
        {
            foreach (var item in Config)
            {
                _provider.Update(item);
            }
        }

        public AlarmZoneConfigViewModel()
        {
            _name = "Zones";
            this._provider = new ZoneConfigurationService(_manager);
        }
        

        [ImportingConstructor]
        public AlarmZoneConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnAlarmZone>();
            this._manager = (AlarmSystemConfigManager)manager;
            this._provider = new ZoneConfigurationService(_manager);
            this._regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/strefy1.png"; }
        }

    }

}
