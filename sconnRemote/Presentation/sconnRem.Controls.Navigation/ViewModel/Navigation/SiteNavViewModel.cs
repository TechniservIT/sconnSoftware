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
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using SiteManagmentService;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{


    [Export]
    public class SiteNavViewModel : BindableBase  
    {
        public ObservableCollection<sconnSite> Sites { get; set; }
        private readonly IRegionManager _regionManager;
        private ISiteRepository _repository;
        public AlarmSystemConfigManager Manager { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public ICommand EditSiteCommand { get; set; }
        public ICommand RemoveSiteCommand { get; set; }
        public ICommand ViewSiteCommand { get; set; }


        private void ViewSite(sconnSite site)
        {
            SiteNavigationManager.ActivateSiteContext(site);

            //navigate context toolbar
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, site.UUID);

            GlobalNavigationContext.NavigateRegionToContractWithParam(
                GlobalViewRegionNames.TopContextToolbarRegion,
                GlobalViewContractNames.Global_Contract_Menu_Top_AlarmSystemContext,
                parameters
                );

            //navigate to global config at start
            this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, AlarmRegionNames.AlarmStatus_Contract_Global_View
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
            //todo
        }

        private void EditSite(sconnSite site)
        {
            SiteNavigationManager.EditSite(site);
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
            Sites = new ObservableCollection<sconnSite>(sconnDataShare.sconnSites.ToArray());
        }


        [ImportingConstructor]
        public SiteNavViewModel(IRegionManager regionManager, ISiteRepository repository)
        {
            this._regionManager = regionManager;
            this._repository = repository;

            SetupCmds();
            Sites = sconnDataShare.sconnSites;
                //new ObservableCollection<sconnSite>(sconnDataShare.sconnSites.ToArray());   //repository.GetAll();   /
        }

    }
    
}
