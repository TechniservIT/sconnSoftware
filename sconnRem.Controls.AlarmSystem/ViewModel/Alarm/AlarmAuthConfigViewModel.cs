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
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition;
using NLog;
using Prism.Mvvm;
using Prism.Mef.Modularity;
using Prism.Modularity;
using sconnRem.Wnd.Config;
using Prism.Regions;

namespace sconnRem.ViewModel.Alarm
{


    [Export]
    public class AlarmAuthConfigViewModel : BindableBase    // ObservableObject, IPageViewModel    //  :  ViewModelBase<IGridNavigatedView>
    {
        public ObservableCollection<sconnAuthorizedDevice> AuthorizedDevices { get; set; }
        private AuthorizedDevicesConfigurationService _provider;
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

                AuthorizedDevices.Clear();
                var retr = _provider.GetAll();
                foreach (var item in retr)
                {
                    AuthorizedDevices.Add(item);
                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex,ex.Message);
            }
        }

        private void SaveData()
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
            AuthorizedDevices = new ObservableCollection<sconnAuthorizedDevice>();
            this._provider = new AuthorizedDevicesConfigurationService(Manager);
        }
        

        [ImportingConstructor]
        public AlarmAuthConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            AuthorizedDevices = new ObservableCollection<sconnAuthorizedDevice>();
            this.Manager = (AlarmSystemConfigManager) manager;
            this._provider = new AuthorizedDevicesConfigurationService(this.Manager);
            this._regionManager = regionManager;
            GetData();
        }
        
    }
}
