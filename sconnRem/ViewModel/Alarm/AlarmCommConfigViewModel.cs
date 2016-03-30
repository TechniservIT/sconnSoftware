using AlarmSystemManagmentService;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;

namespace sconnRem.ViewModel.Alarm
{


    [Export]
    public class AlarmCommConfigViewModel : BindableBase 
    {
        public sconnGlobalConfig CommConfig { get; set; }

        private GlobalConfigService _provider;
        private readonly IRegionManager _regionManager;

        private Logger _nlogger = LogManager.GetCurrentClassLogger();


        [Dependency]
        public AlarmSystemConfigManager Manager { get; set; }


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
                CommConfig = _provider.Get();
                
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            _provider.Update(CommConfig);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }

        
        public AlarmCommConfigViewModel()
        {
            _name = "Auth";
            this._provider = new GlobalConfigService(Manager);
        }
        

        [ImportingConstructor]
        public AlarmCommConfigViewModel(IRegionManager regionManager)
        {
            this._regionManager = regionManager;

        }

        [ImportingConstructor]
        public AlarmCommConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            CommConfig = new sconnGlobalConfig();
            this.Manager = (AlarmSystemConfigManager)manager;
            this._provider = new GlobalConfigService(this.Manager);
            this._regionManager = regionManager;
            GetData();
        }


    }
    
}
