using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnPrismSharedContext;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    public class AlarmInputConfigViewModel : BindableBase  //ObservableObject, IPageViewModel
    {
        public sconnInput Config { get; set; }
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

        public ICommand NavigateBackCommand { get; set; }

        private void GetData()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
          
        }

        public AlarmInputConfigViewModel()
        {
            this._manager = AlarmSystemContext.GetManager();
            _name = "Dev";
            this._provider = new DeviceConfigService(_manager);
        }

        public void NavigateBack()
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, AlarmRegionNames.AlarmStatus_Contract_Device_List_View
                    ,
                    (NavigationResult nr) =>
                    {
                        var error = nr.Error;
                        var result = nr.Result;
                        if (error != null)
                        {
                            _nlogger.Error(error);
                        }
                    });
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }

        [ImportingConstructor]
        public AlarmInputConfigViewModel(sconnInput input, IAlarmConfigManager manager, IRegionManager regionManager)
        {
            SetupCmds();
            Config = input;
            this._manager = (AlarmSystemConfigManager)manager;
            this._regionManager = regionManager;
            GetData();
        }

        private void SetupCmds()
        {
            NavigateBackCommand = new DelegateCommand(NavigateBack);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

    }



}
