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

        private GlobalConfigService _Provider;
        private readonly IRegionManager regionManager;

        private Logger nlogger = LogManager.GetCurrentClassLogger();


        [Dependency]
        public AlarmSystemConfigManager _Manager { get; set; }


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
                CommConfig = _Provider.Get();
                
            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            _Provider.Update(CommConfig);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }

        
        public AlarmCommConfigViewModel()
        {
            _Name = "Auth";
            this._Provider = new GlobalConfigService(_Manager);
        }
        

        [ImportingConstructor]
        public AlarmCommConfigViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

        }

        [ImportingConstructor]
        public AlarmCommConfigViewModel(IAlarmConfigManager Manager, IRegionManager regionManager)
        {
            CommConfig = new sconnGlobalConfig();
            this._Manager = (AlarmSystemConfigManager)Manager;
            this._Provider = new GlobalConfigService(_Manager);
            this.regionManager = regionManager;
            GetData();
        }


    }
    
}
