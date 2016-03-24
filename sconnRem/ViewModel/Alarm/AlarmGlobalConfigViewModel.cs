using AlarmSystemManagmentService;
using Prism.Mvvm;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmGlobalConfigViewModel  : BindableBase // ObservableObject, IPageViewModel    //:  ViewModelBase<IGridNavigatedView>  
    {
        public AlarmSystemGlobalConfig Config { get; set; }
        private GlobalConfigService _Provider;
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
            this._Provider = new GlobalConfigService(_Manager);
        }

        [ImportingConstructor]
        public AlarmGlobalConfigViewModel(AlarmSystemConfigManager Manager)
        {
            _Manager = Manager;
            _Name = "Gcfg";
            this._Provider = new GlobalConfigService(_Manager);

        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config2.png"; }
        }

    }

}
