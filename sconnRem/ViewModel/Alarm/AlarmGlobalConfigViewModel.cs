using AlarmSystemManagmentService;
using Prism.Mvvm;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmGlobalConfigViewModel  : BindableBase // ObservableObject, IPageViewModel    //:  ViewModelBase<IGridNavigatedView>  
    {
        public sconnGlobalConfig Config { get; set; }
        private GlobalConfigService _provider;
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

        public AlarmGlobalConfigViewModel()
        {
            _name = "Gcfg";
            this._provider = new GlobalConfigService(_manager);
        }

      

        [ImportingConstructor]
        public AlarmGlobalConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            Config = new sconnGlobalConfig();
            this._manager = (AlarmSystemConfigManager)manager;
            this._provider = new GlobalConfigService(_manager);
            this._regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }

    }

}
