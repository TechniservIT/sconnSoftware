using AlarmSystemManagmentService;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmGlobalConfigViewModel  : ObservableObject, IPageViewModel    //:  ViewModelBase<IGridNavigatedView>  
    {
        public AlarmSystemAuthorizedDevicesConfig Config { get; set; }
        private AuthorizedDevicesConfigurationService _Provider;
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
        public AlarmGlobalConfigViewModel()
        {
            _Name = "Gcfg";
        }

        //public AlarmGlobalConfigViewModel(IGridNavigatedView view) : base(view)
        //{
        //    this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        //}
    }

}
