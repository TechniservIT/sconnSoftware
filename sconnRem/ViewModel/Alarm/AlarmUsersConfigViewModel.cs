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
        private UsersConfigurationService _Provider;
        private AlarmSystemConfigManager _Manager;
        private readonly IRegionManager regionManager;
        private Logger nlogger = LogManager.GetCurrentClassLogger();

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
                Config = new ObservableCollection<sconnUser>(_Provider.GetAll());

            }
            catch (Exception ex)
            {
                nlogger.Error(ex, ex.Message);
            }
        }

        private void SaveData()
        {
            foreach (var item in Config)
            {
                _Provider.Update(item);
            }
        }

        public AlarmUsersConfigViewModel()
        {
            _Name = "Users";
            this._Provider = new UsersConfigurationService(_Manager);
        }


        [ImportingConstructor]
        public AlarmUsersConfigViewModel(IAlarmConfigManager Manager, IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnUser>();
            this._Manager = (AlarmSystemConfigManager)Manager;
            this._Provider = new UsersConfigurationService(_Manager);
            this.regionManager = regionManager;
            GetData();
        }


        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/user.png"; }
        }

    }

}
