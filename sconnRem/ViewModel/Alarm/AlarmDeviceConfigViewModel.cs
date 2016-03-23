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

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmDeviceConfigViewModel : ObservableObject, IPageViewModel
    {
        public AlarmSystemDeviceConfig Config { get; set; }
        private DeviceConfigService _Provider;
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
        
        public AlarmDeviceConfigViewModel()
        {
            _Name = "Dev";
            this._Provider = new DeviceConfigService(_Manager);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

    }
}
