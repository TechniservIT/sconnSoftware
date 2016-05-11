using AlarmSystemManagmentService;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using System.Windows.Input;
using sconnConnector.POCO.Config.sconn;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using NLog;
using Prism;
using Prism.Mvvm;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{


    [Export]
    public class AlarmAuthConfigViewModel : GenericAsyncConfigViewModel
    {
        public ObservableCollection<sconnAuthorizedDevice> Config { get; set; }
        private AuthorizedDevicesConfigurationService _provider;
        public AlarmSystemConfigManager Manager { get; set; }
        
        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        
        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnAuthorizedDevice>(_provider.GetAll());
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex,ex.Message);
            }
        }

        public  override  void SaveData()
        {
            _provider.SaveChanges();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }

        
        public AlarmAuthConfigViewModel()
        {
            _name = "Auth";
            Config = new ObservableCollection<sconnAuthorizedDevice>();
            this._provider = new AuthorizedDevicesConfigurationService(Manager);
        }
        

        [ImportingConstructor]
        public AlarmAuthConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnAuthorizedDevice>();
            this.Manager = (AlarmSystemConfigManager) manager;
            this._provider = new AuthorizedDevicesConfigurationService(this.Manager);
            this._regionManager = regionManager;
            GetData();
        }
        

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_AuthConfigView))
            {
                return true;    //singleton
            }
            return false;
        }


    }
}
