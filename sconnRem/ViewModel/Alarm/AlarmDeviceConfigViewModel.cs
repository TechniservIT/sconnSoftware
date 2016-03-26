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
using Prism.Mvvm;
using System.ComponentModel.Composition;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    public class AlarmDeviceConfigViewModel : BindableBase  //ObservableObject, IPageViewModel
    {
        public sconnDevice Config { get; set; }
        private DeviceConfigService _Provider;
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
                Config = _Provider.Get();

            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            _Provider.Update(Config);
        }

        public AlarmDeviceConfigViewModel()
        {
            _Name = "Dev";
            this._Provider = new DeviceConfigService(_Manager);
        }

        [ImportingConstructor]
        public AlarmDeviceConfigViewModel(AlarmSystemConfigManager Manager)
        {
            _Manager = Manager;
            _Name = "Dev";
            this._Provider = new DeviceConfigService(_Manager);

        }

        [ImportingConstructor]
        public AlarmDeviceConfigViewModel(IAlarmConfigManager Manager, IRegionManager regionManager)
        {
            Config = new sconnDevice();
            this._Manager = (AlarmSystemConfigManager)Manager;
            this._Provider = new DeviceConfigService(_Manager);
            this.regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

    }
}
