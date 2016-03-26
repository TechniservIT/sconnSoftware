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
        private GlobalConfigService _Provider;
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

        public AlarmGlobalConfigViewModel()
        {
            _Name = "Gcfg";
            this._Provider = new GlobalConfigService(_Manager);
        }

      

        [ImportingConstructor]
        public AlarmGlobalConfigViewModel(IAlarmConfigManager Manager, IRegionManager regionManager)
        {
            Config = new sconnGlobalConfig();
            this._Manager = (AlarmSystemConfigManager)Manager;
            this._Provider = new GlobalConfigService(_Manager);
            this.regionManager = regionManager;
            GetData();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }

    }

}
