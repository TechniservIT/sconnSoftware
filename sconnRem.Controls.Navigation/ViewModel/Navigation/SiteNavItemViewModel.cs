using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{

    [Export]
    public class SiteNavItemViewModel : BindableBase
    {
        public sconnSite Site { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }
        
        //private ICommand _getDataCommand;
        //private ICommand _saveDataCommand;
        
        public SiteNavItemViewModel(sconnSite site)
        {
            Site = site;
        }


        [ImportingConstructor]
        public SiteNavItemViewModel(IRegionManager regionManager)
        {
            this._regionManager = regionManager;
        }

    }

    
}
