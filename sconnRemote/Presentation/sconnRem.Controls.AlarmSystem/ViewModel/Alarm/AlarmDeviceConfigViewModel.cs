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
using Prism.Mvvm;
using System.ComponentModel.Composition;
using NLog;
using Prism;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    public class AlarmDeviceConfigViewModel : GenericAsyncConfigViewModel
    {
        public sconnDevice Config { get; set; }
        private DeviceConfigService _provider;
        private AlarmSystemConfigManager _manager;
        private readonly IRegionManager _regionManager;
     
        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        public override void GetData()
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

        public override void SaveData()
        {
            _provider.Update(Config);
        }

        public AlarmDeviceConfigViewModel()
        {
            _name = "Dev";
            this._provider = new DeviceConfigService(_manager);
        }

        [ImportingConstructor]
        public AlarmDeviceConfigViewModel(AlarmSystemConfigManager manager)
        {
            _manager = manager;
            _name = "Dev";
            this._provider = new DeviceConfigService(_manager);

        }

        [ImportingConstructor]
        public AlarmDeviceConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            Config = new sconnDevice();
            this._manager = (AlarmSystemConfigManager)manager;
            this._provider = new DeviceConfigService(_manager);
            this._regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

       
    }
}
