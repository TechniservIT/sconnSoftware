using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.SiteManagment.Wizard;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using SiteManagmentService;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{


    [Export]
    public class SiteNavViewModel : BindableBase  
    {

        public ObservableCollection<sconnSite> Sites { get; set; }
        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public ICommand EditSiteCommand { get; set; }
        public ICommand RemoveSiteCommand { get; set; }
        public ICommand ViewSiteCommand { get; set; }

        public ICommand SearchSitesCommand { get; set; }
        
        private void ViewSite(sconnSite site)
        {
            try
            {
                AlarmNavService.ActivateSiteContext(site);

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
            catch (Exception ex)
            {
                _nlogger.Error(ex,ex.Message);
            }
          
        }

        private void RemoveSite(sconnSite site)
        {
            AlarmNavService.RemoveSite(site);
        }

        private void EditSite(sconnSite site)
        {
            AlarmNavService.EditSite(site);
        }

        private void SearchSites()
        {
            AlarmNavService.Stage = SiteConnectionWizardStage.Search;
            AlarmNavService.OpenSiteWizard();
        }

        private void SetupCmds()
        {
            EditSiteCommand = new DelegateCommand<sconnSite>(EditSite);
            RemoveSiteCommand = new DelegateCommand<sconnSite>(RemoveSite);
            ViewSiteCommand = new DelegateCommand<sconnSite>(ViewSite);
            SearchSitesCommand = new DelegateCommand(SearchSites);
        }
        

        [ImportingConstructor]
        public SiteNavViewModel(IRegionManager regionManager, ISiteRepository repository, IAlarmSystemNavigationService NavService)
        {
            try
            {
                this._regionManager = regionManager;
                AlarmNavService = NavService;
                SetupCmds();
                Sites = sconnDataShare.sconnSites;
                Sites.CollectionChanged += Sites_CollectionChanged;
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        private void Sites_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StopSiteConnectivityCheckTimer();   //stop existing first if any
            StartSiteConnectivityCheckTimer();
        }

        public bool ConnectivityCheckOnline { get; set; }
        public CancellationToken SiteConnectivityCheckCancellationToken { get; set; }
        public CancellationTokenSource SiteConnectivityCheckCancellationTokenSrc { get; set; }

        public void StartSiteConnectivityCheckTimer()
        {
            try
            {
                SiteConnectivityCheckCancellationTokenSrc = new CancellationTokenSource();
                SiteConnectivityCheckCancellationToken = SiteConnectivityCheckCancellationTokenSrc.Token;
                ScheduleSitesConnectivityCheckInSeconds(5, SiteConnectivityCheckCancellationToken);
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        public void StopSiteConnectivityCheckTimer()
        {
            try
            {
                SiteConnectivityCheckCancellationTokenSrc?.Cancel();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public async void ScheduleSitesConnectivityCheckInSeconds(int seconds, CancellationToken cancelToken)
        {
            try
            {
                while (!cancelToken.IsCancellationRequested) // ConnectivityCheckOnline
                {
                    foreach (var checkedSite in Sites)
                    {
                        try
                        {
                            await PerformSiteConnectivityBackgroundCheck(checkedSite, cancelToken);
                        }
                        catch (Exception ex)
                        {
                            _nlogger.Error(ex, ex.Message);
                        }
                    }
                    await Task.Delay(seconds*1000, cancelToken);
                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        public Task PerformSiteConnectivityBackgroundCheck(sconnSite site, CancellationToken cancelToken)
        {
            try
            {
                Action mainAction = () =>
                {
                    if (cancelToken.IsCancellationRequested) { return; }

                    SconnClient cl = new SconnClient(site.serverIP, site.serverPort, site.authPasswd, true);
                    if (cl.Connect())
                    {
                        if (cl.Authenticated)
                        {
                            //update site stat in main thread
                            Application.Current.Dispatcher.Invoke(() => { site.SiteStat.Authenticated = true; });
                        }
                    }
                    cl.Disconnect();
                };
                return Task.Factory.StartNew(mainAction, cancelToken);
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
                return null;
            }

        }


    }

}
