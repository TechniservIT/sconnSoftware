using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using sconnConnector.POCO.Config.Abstract.Auth;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmUsersConfigViewModel : GenericAsyncConfigViewModel
    {

        private ObservableCollection<sconnUser> _config;
        public ObservableCollection<sconnUser> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }


        private UsersConfigurationService _provider;
        private AlarmSystemConfigManager _manager;
       
        
        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnUser>(_provider.GetAll());

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
            foreach (var item in Config)
            {
                _provider.Update(item);
            }
        }

        public AlarmUsersConfigViewModel()
        {
            _name = "Users";
            this._provider = new UsersConfigurationService(_manager);
        }
        

        [ImportingConstructor]
        public AlarmUsersConfigViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnUser>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new UsersConfigurationService(_manager);
            this._regionManager = regionManager;
            GetData();
        }
        
        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/user.png"; }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_UsersConfigView))
            {
                return true;    //singleton
            }
            return false;
        }


    }

}
