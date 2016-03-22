using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmGsmConfigViewModel : GsmConfigurationService
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

        public AlarmGsmConfigViewModel()
        {
            _Name = "Auth";
        }
    }
}
