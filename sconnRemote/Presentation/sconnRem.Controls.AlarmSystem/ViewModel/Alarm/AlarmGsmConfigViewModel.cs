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
using sconnConnector.POCO.Config;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Infrastructure.Navigation;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    public class AlarmGsmConfigViewModel : BindableBase     // ObservableObject, IPageViewModel
    {
        public ObservableCollection<sconnGsmRcpt> Config { get; set; }
        private GsmConfigurationService _provider;
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
                Config = new ObservableCollection<sconnGsmRcpt>(_provider.GetAll());

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

        public AlarmGsmConfigViewModel()
        {
            _name = "Gsm";
            this._provider = new GsmConfigurationService(_manager);
        }

                  

            

        [ImportingConstructor]
        public AlarmGsmConfigViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnGsmRcpt>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new GsmConfigurationService(_manager);
            this._regionManager = regionManager;
            GetData();
        }



        //[ImportingConstructor]
        //public AlarmGsmConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        //{
        //    Config = new ObservableCollection<sconnGsmRcpt>();
        //    this._manager = (AlarmSystemConfigManager)manager;
        //    this._provider = new GsmConfigurationService(_manager);
        //    this._regionManager = regionManager;
        //    GetData();
        //}



        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/tel.png"; }
        }

    }
}
