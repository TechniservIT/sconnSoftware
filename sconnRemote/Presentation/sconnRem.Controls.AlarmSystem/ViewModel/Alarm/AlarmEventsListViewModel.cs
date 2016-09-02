using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService.Device;
using AlarmSystemManagmentService.Event;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmEventsListViewModel : GenericAlarmConfigViewModel
    {
        private ObservableCollection<sconnEvent> _config;
        private EventsService _provider;

        public ObservableCollection<sconnEvent> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand ShowDeviceStatusCommand { get; set; }
        

        public override void GetData()
        {
            try
            {
                if (SiteNavigationManager.Online)
                {
                    Config = new ObservableCollection<sconnEvent>(_provider.GetAll());
                }
                else
                {
                    Config = new ObservableCollection<sconnEvent>(SiteNavigationManager.alarmSystemConfigManager.Config.EventConfig.Events);
                }
             
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {

        }

        public AlarmEventsListViewModel()
        {
            Config = new ObservableCollection<sconnEvent>(new List<sconnEvent>());
            _name = "Gcfg";
            this._provider = new EventsService(_manager);
        }


        [ImportingConstructor]
        public AlarmEventsListViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnEvent>(new List<sconnEvent>());
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new EventsService(_manager);
            _regionManager = regionManager;
            this.PropertyChanged += new PropertyChangedEventHandler(OnNotifiedOfPropertyChanged);
        }

        private void OnNotifiedOfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && !String.Equals(e.PropertyName, "IsChanged", StringComparison.Ordinal))
            {
                this.IsChanged = true;
            }
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }


        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if ( navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmStatus_Contract_EventsView) )
            {
                return true;    //singleton
            }
            return false;
        }

    }


}
