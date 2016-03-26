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

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    public class AlarmGsmConfigViewModel : BindableBase     // ObservableObject, IPageViewModel
    {
        public ObservableCollection<sconnGsmRcpt> Config { get; set; }
        private GsmConfigurationService _Provider;
        private AlarmSystemConfigManager _Manager;
        private readonly IRegionManager regionManager;
        private Logger nlogger = LogManager.GetCurrentClassLogger();

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        private void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnGsmRcpt>(_Provider.GetAll());

            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            foreach (var item in Config)
            {
                _Provider.Update(item);
            }
        }

        public AlarmGsmConfigViewModel()
        {
            _Name = "Gsm";
            this._Provider = new GsmConfigurationService(_Manager);
        }

       

        [ImportingConstructor]
        public AlarmGsmConfigViewModel(IAlarmConfigManager Manager, IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnGsmRcpt>();
            this._Manager = (AlarmSystemConfigManager)Manager;
            this._Provider = new GsmConfigurationService(_Manager);
            this.regionManager = regionManager;
            GetData();
        }



        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/tel.png"; }
        }

    }
}
