using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Navigation;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{


    [Export]
    public class SiteNavViewModel : BindableBase  
    {
        public ObservableCollection<sconnSite> Sites { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public ICommand EditSiteCommand { get; set; }
        public ICommand RemoveSiteCommand { get; set; }
        public ICommand ViewSiteCommand { get; set; }


        private void ViewSite(sconnSite site)
        {
            this._regionManager.RequestNavigate(GlobalViewRegionNames.TopContextToolbarRegion, NavContextToolbarRegionNames.ContextToolbar_AlarmSystem_ViewUri
                ,
                (NavigationResult nr) =>
                {
                    var error = nr.Error;
                    var result = nr.Result;
                    if (error != null)
                    {
                        _nlogger.Error(error);
                    }
                });
        }

        private void RemoveSite(sconnSite site)
        {

        }

        private void EditSite(sconnSite site)
        {

        }


        private void SetupCmds()
        {
            EditSiteCommand = new DelegateCommand<sconnSite>(EditSite);
            RemoveSiteCommand = new DelegateCommand<sconnSite>(RemoveSite);
            ViewSiteCommand = new DelegateCommand<sconnSite>(ViewSite);
        }

        public SiteNavViewModel()
        {
            SetupCmds();
            Sites = new ObservableCollection<sconnSite>(sconnDataShare.getSites());
        }


        [ImportingConstructor]
        public SiteNavViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            Sites = new ObservableCollection<sconnSite>(sconnDataShare.getSites());
            this._regionManager = regionManager;
        }

    }
    
}
