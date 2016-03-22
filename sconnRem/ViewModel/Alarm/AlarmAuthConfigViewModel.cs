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

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmAuthConfigViewModel : ObservableObject, IPageViewModel    //  :  ViewModelBase<IGridNavigatedView>
    {
        public AlarmSystemAuthorizedDevicesConfig Config { get; set; }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        private AuthorizedDevicesConfigurationService _Provider;
        private AlarmSystemConfigManager _Manager;

        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        private void GetData()
        {
        }

        private void SaveData()
        {

        }

        //public AlarmAuthConfigViewModel(IGridNavigatedView view) : base(view)
        //{
        //    this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        //}

        public AlarmAuthConfigViewModel()
        {
            _Name = "Auth";
        }

        //public AlarmAuthConfigViewModel(AlarmSystemConfigManager manager)
        //{
        //    this._Manager = manager;
        //    this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        //}


    }

}
