using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
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

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmUsersConfigViewModel : BindableBase   // ObservableObject, IPageViewModel
    {
        public sconnUserConfig Config { get; set; }
        private UsersConfigurationService _Provider;
        private AlarmSystemConfigManager _Manager;

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
        }

        private void SaveData()
        {

        }

        public AlarmUsersConfigViewModel()
        {
            _Name = "Users";
            this._Provider = new UsersConfigurationService(_Manager);
        }

        [ImportingConstructor]
        public AlarmUsersConfigViewModel(AlarmSystemConfigManager Manager)
        {
            _Manager = Manager;
            _Name = "Users";
            this._Provider = new UsersConfigurationService(_Manager);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/user.png"; }
        }

    }

}
