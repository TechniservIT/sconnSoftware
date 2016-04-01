using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{


    [Export]
    public class SiteNavViewModel : BindableBase  
    {
        public ObservableCollection<sconnSite> Sites { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }
        

        //private ICommand _getDataCommand;
        //private ICommand _saveDataCommand;
        

        public SiteNavViewModel()
        {
            Sites = new ObservableCollection<sconnSite>(sconnDataShare.getSites());
        }


        [ImportingConstructor]
        public SiteNavViewModel(IRegionManager regionManager)
        {
            Sites = new ObservableCollection<sconnSite>(sconnDataShare.getSites());
            this._regionManager = regionManager;
        }

    }
    
}
