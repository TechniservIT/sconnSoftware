using AlarmSystemManagmentService;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sconnRem.ViewModel.Alarm
{


    [Export]
    public class AlarmCommConfigViewModel : BindableBase 
    {
        public sconnGlobalConfig CommConfig { get; set; }

        private AuthorizedDevicesConfigurationService _Provider;

        [Dependency]
        public AlarmSystemConfigManager _Manager { get; set; }


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

  

        private void SaveData()
        {
            _Provider.SaveChanges();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }

        
        public AlarmCommConfigViewModel()
        {
            _Name = "Auth";
            this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        }

        [ImportingConstructor]
        public AlarmCommConfigViewModel(AlarmSystemConfigManager Manager)
        {
            _Manager = Manager;
            _Name = "Auth";
            this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
            //  GetData();
        }


    }
    
}
