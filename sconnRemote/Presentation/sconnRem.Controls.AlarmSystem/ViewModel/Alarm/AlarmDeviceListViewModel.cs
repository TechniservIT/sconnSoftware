using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using NLog;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    public class AlarmDeviceListViewModel : BindableBase
    {
        public List<sconnDevice> Config { get; set; }
        private AlarmDevicesConfigService _provider;
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
                Config = _provider.GetAll();

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
          //  _provider.Update(Config);
        }

        public AlarmDeviceListViewModel()
        {
            _name = "Gcfg";
            this._provider = new AlarmDevicesConfigService(_manager);
        }



        [ImportingConstructor]
        public AlarmDeviceListViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            Config = new List<sconnDevice>();
            this._manager = (AlarmSystemConfigManager)manager;
            this._provider = new AlarmDevicesConfigService(_manager);
            this._regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }

    }


}