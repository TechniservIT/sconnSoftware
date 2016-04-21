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
using AlarmSystemManagmentService.Device;
using Prism;
using Prism.Commands;
using sconnConnector.POCO.Config;
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{
    
    [Export]
    public class AlarmSharedDeviceConfigViewModel : BindableBase, IActiveAware   //ObservableObject, IPageViewModel
    {
        public sconnDevice Config { get; set; }

        public sconnInput ActiveInput { get; set; }
        public sconnOutput ActiveOutput { get; set; }
        public sconnRelay ActiveRelay { get; set; }

        private DeviceConfigService _provider;
        private AlarmSystemConfigManager _manager;
        private readonly IRegionManager _regionManager;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        private string _name;
        private bool _isActive;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public ICommand NavigateBackCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    GetData();
                }
            }
        }

        public event EventHandler IsActiveChanged;

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
            //copy back activated IO
            try
            {
                if (ActiveInput != null)
                {
                    sconnInput existing = Config.Inputs.First(i => i.Id == ActiveInput.Id);
                    existing?.CopyFrom(ActiveInput);
                }

                if (ActiveOutput != null)
                {
                    sconnOutput existing = Config.Outputs.First(i => i.Id == ActiveOutput.Id);
                    existing?.CopyFrom(ActiveOutput);
                }


                if (ActiveRelay != null)
                {
                    sconnRelay existing = Config.Relays.First(i => i.Id == ActiveRelay.Id);
                    existing?.CopyFrom(ActiveRelay);
                }


            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
          
            _provider.Update(Config);
        }

        
        
        public AlarmSharedDeviceConfigViewModel()
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
        public AlarmSharedDeviceConfigViewModel(IRegionManager regionManager)  //sconnDevice device, 
        {
            SetupCmds();
            Config = SiteNavigationManager.CurrentContextDevice;
            ActiveInput = SiteNavigationManager.activeInput;
            this._manager = SiteNavigationManager.alarmSystemConfigManager; // (AlarmSystemConfigManager)manager;
            this._provider = new DeviceConfigService(_manager, Config.DeviceId);
            this._regionManager = regionManager;
            GetData();
        }

        private void SetupCmds()
        {
            NavigateBackCommand = new DelegateCommand(NavigateBack);
            SaveCommand = new DelegateCommand(SaveData);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

    }

}
