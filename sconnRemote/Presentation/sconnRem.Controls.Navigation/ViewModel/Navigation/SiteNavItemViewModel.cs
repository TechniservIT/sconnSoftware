using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
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
        
        public ICommand EditSiteCommand { get; set; }
        public ICommand RemoveSiteCommand { get; set; }
        public ICommand ViewSiteCommand { get; set; }


        private void ViewSite()
        {

        }

        private void RemoveSite()
        {

        }

        private void EditSite()
        {
           
        }



        public SiteNavItemViewModel(sconnSite site)
        {

            EditSiteCommand = new DelegateCommand(EditSite);
            RemoveSiteCommand = new DelegateCommand(RemoveSite);
            ViewSiteCommand = new DelegateCommand(ViewSite);

            Site = site;
        }


        [ImportingConstructor]
        public SiteNavItemViewModel(IRegionManager regionManager)
        {
            EditSiteCommand = new DelegateCommand(EditSite);
            RemoveSiteCommand = new DelegateCommand(RemoveSite);
            ViewSiteCommand = new DelegateCommand(ViewSite);

            this._regionManager = regionManager;
        }

        public SiteNavItemViewModel()
        {
            EditSiteCommand = new DelegateCommand(EditSite);
            RemoveSiteCommand = new DelegateCommand(RemoveSite);
            ViewSiteCommand = new DelegateCommand(ViewSite);
        }
    }

    
}
