using AlarmSystemManagmentService;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmAuthConfigViewModel       /* :  ViewModelBase<IGridNavigatedView>*/
    {
        public AlarmSystemAuthorizedDevicesConfig Config { get; set; }
        private AuthorizedDevicesConfigurationService _Provider;
        private AlarmSystemConfigManager _Manager;
        

        //public AlarmAuthConfigViewModel(IGridNavigatedView view) : base(view)
        //{
        //    this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        //    AlarmSystemAuthorizedDevicesModel model = new AlarmSystemAuthorizedDevicesModel(_Provider.GetAll());
        //}

        public AlarmAuthConfigViewModel()
        {
           
        }

        public AlarmAuthConfigViewModel(AlarmSystemConfigManager manager)
        {
            this._Manager = manager;
            this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        }


    }

}
