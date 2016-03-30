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

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmUsersConfigViewModel : BindableBase   // ObservableObject, IPageViewModel
    {
        public ObservableCollection<sconnUser> Config { get; set; }
        private UsersConfigurationService _provider;
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



        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        private void GetData()
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

        private void SaveData()
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
        public AlarmUsersConfigViewModel(IAlarmConfigManager manager, IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnUser>();
            this._manager = (AlarmSystemConfigManager)manager;
            this._provider = new UsersConfigurationService(_manager);
            this._regionManager = regionManager;
            GetData();
        }


        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/user.png"; }
        }

    }

}
