using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using NLog;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using System.ComponentModel.Composition.Primitives;
using sconnPrismSharedContext;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{
    
    [Export]
    public class AlarmSharedDeviceConfigViewModel : BindableBase  //ObservableObject, IPageViewModel
    {
        public sconnDevice Config { get; set; }
        private DeviceConfigService _provider;
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
                Config = _provider.Get();

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            _provider.Update(Config);
        }
        
        public AlarmSharedDeviceConfigViewModel()
        {
            this._manager = AlarmSystemContext.GetManager();
            _name = "Dev";
            this._provider = new DeviceConfigService(_manager);
        }

        //[ImportingConstructor]
        //public AlarmSharedDeviceConfigViewModel(IAlarmConfigManager manager)
        //{
        //    // _manager = manager;
        //    this._manager = AlarmSystemContext.GetManager();
        //    _name = "Dev";
        //    this._provider = new DeviceConfigService(_manager);

        //}

        [ImportingConstructor]
        public AlarmSharedDeviceConfigViewModel(IRegionManager regionManager)
        {
            Config = new sconnDevice();
            this._manager = AlarmSystemContext.GetManager();
            this._provider = new DeviceConfigService(_manager);
            this._regionManager = regionManager;
            GetData();


        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

    }

}
