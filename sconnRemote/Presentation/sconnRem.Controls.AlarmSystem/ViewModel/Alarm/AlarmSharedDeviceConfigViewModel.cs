using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{
    
    [Export]
    public class AlarmSharedDeviceConfigViewModel : GenericAsyncConfigViewModel
    {

        private sconnDevice _Config = new sconnDevice();
        public sconnDevice Config
        {
            get { return _Config; }
            set { SetProperty(ref _Config, value); }
        }

        private sconnInput _ActiveInput = new sconnInput();
        public sconnInput ActiveInput
        {
            get { return _ActiveInput; }
            set { SetProperty(ref _ActiveInput, value); }
        }

        private sconnOutput _ActiveOutput = new sconnOutput();
        public sconnOutput ActiveOutput
        {
            get { return _ActiveOutput; }
            set { SetProperty(ref _ActiveOutput, value); }
        }

        private sconnRelay _ActiveRelay = new sconnRelay();
        public sconnRelay ActiveRelay
        {
            get { return _ActiveRelay; }
            set { SetProperty(ref _ActiveRelay, value); }
        }
        
        public int ChangeTrack { get; set; }

        private DeviceConfigService _provider;
        private AlarmSystemConfigManager _manager;
        private bool _isActive;

        public void UpdateActiveIo()
        {
            ChangeTrack++;
            this.Config.CopyFrom(SiteNavigationManager.CurrentContextDevice);

            this.ActiveInput.CopyFrom(SiteNavigationManager.activeInput);

            this.ActiveOutput.CopyFrom(SiteNavigationManager.activeOutput);

            this.ActiveRelay.CopyFrom(SiteNavigationManager.activeRelay);


            OnPropertyChanged();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            UpdateActiveIo();

            return true;

        }
        
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateActiveIo();
        }

        public ICommand NavigateBackCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public override void GetData()
        {
            try
            {
               // Config = _provider.Get();
               Config.CopyFrom(_provider.Get());

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
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
            UpdateActiveIo();
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
