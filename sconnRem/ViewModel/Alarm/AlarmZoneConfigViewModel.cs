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
using sconnConnector.POCO.Config.sconn;
using Prism.Mvvm;
using System.ComponentModel.Composition;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmZoneConfigViewModel : BindableBase    // ObservableObject, IPageViewModel
    {
        public sconnAlarmZoneConfig Config { get; set; }
        private ZoneConfigurationService _Provider;
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

        public AlarmZoneConfigViewModel()
        {
            _Name = "Zones";
            this._Provider = new ZoneConfigurationService(_Manager);
        }

        [ImportingConstructor]
        public AlarmZoneConfigViewModel(AlarmSystemConfigManager Manager)
        {
            _Manager = Manager;
            _Name = "Zones";
            this._Provider = new ZoneConfigurationService(_Manager);
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/strefy1.png"; }
        }

    }

}
