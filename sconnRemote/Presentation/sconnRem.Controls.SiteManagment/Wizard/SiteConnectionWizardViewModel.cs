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

namespace sconnRem.Controls.SiteManagment.Wizard
{
    public enum SiteConnectionWizardStage
    {
        MethodSelection,
        Search,
        UsbList,
        ManualEntry,
        Test,
        Summary
    }

    public enum SiteAdditionMethod
    {
        Manual,
        Search,
        UsbList
    }

    [Export]
    public class SiteConnectionWizardViewModel : BindableBase
    {
        public sconnSite Config { get; set; }
        private readonly IRegionManager _regionManager;
        private SconnClient _provider;

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public SiteConnectionWizardStage Stage { get; set; }
        public SiteAdditionMethod AdditionMethod { get; set; }
        
        public ICommand NavigateBackCommand { get; set; }
        public ICommand NavigateForwardCommand { get; set; }

        public ICommand OpenSearchViewCommand { get; set; }
        public ICommand OpenManualEntryViewCommand { get; set; }
        public ICommand OpenUsbListViewCommand { get; set; }

        public ICommand SaveSiteCommand { get; set; }
        public ICommand VerifyConnectionCommand { get; set; }

        public ObservableCollection<sconnSite> NetworkSites { get; set; }
        public ObservableCollection<sconnSite> UsbSites { get; set; }

        private void NavigateToContract(string contract)
        {
            try
            {
                this._regionManager.RequestNavigate(SiteManagmentRegionNames.MainContentRegion, contract
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
                _nlogger.Error(ex, ex.Message);

            }
        }

        private void SearchForSitesInNetwork()
        {
            //  NetworkSites
            this.NetworkSites = new ObservableCollection<sconnSite>();
            _provider.SearchForSite();
        }

        private void _provider_SiteDiscovered(object sender, EventArgs e)
        {
            SiteDiscoveryEventArgs args = (SiteDiscoveryEventArgs) e;
            this.NetworkSites.Add(new sconnSite(500,args.hostname,9898, args.hostname));
        }

        private void NavigateBack()
        {
            if (Stage == SiteConnectionWizardStage.MethodSelection)
            {
                //no action - cannot navigate back
            }
            else if (Stage == SiteConnectionWizardStage.Search ||
                Stage == SiteConnectionWizardStage.ManualEntry ||
                Stage == SiteConnectionWizardStage.UsbList)
            {
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_MethodSelection_View);
            }
            else if (Stage == SiteConnectionWizardStage.Test)
            {
                //find out
                if (AdditionMethod == SiteAdditionMethod.Search)
                {
                    this.Stage = SiteConnectionWizardStage.Search;
                    SearchForSitesInNetwork(); // start network search
                    NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_SearchSitesList_View);
                }
                else if (AdditionMethod == SiteAdditionMethod.Manual)
                {
                    this.Stage = SiteConnectionWizardStage.ManualEntry;
                    NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View);
                }
                else if (AdditionMethod == SiteAdditionMethod.UsbList)
                {
                    this.Stage = SiteConnectionWizardStage.UsbList;
                    NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_UsbList_View);
                }
            }
            else if (Stage == SiteConnectionWizardStage.Summary)
            {
                this.Stage = SiteConnectionWizardStage.Test;
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_Test_View);
            }
        }

        private void NavigateForward()
        {
            if (Stage == SiteConnectionWizardStage.MethodSelection)
            {
                //no action - cannot navigate forward
            }
            else if (Stage == SiteConnectionWizardStage.Search ||
                Stage == SiteConnectionWizardStage.ManualEntry ||
                Stage == SiteConnectionWizardStage.UsbList 
                )
            {
                this.Stage = SiteConnectionWizardStage.Test;
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_Test_View);
            }
            else if (Stage == SiteConnectionWizardStage.Test)
            {
                this.Stage = SiteConnectionWizardStage.Summary;
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_Summary_View);
                VerifyConnection();
            }
            else if (Stage == SiteConnectionWizardStage.Summary)
            {
                //end - save and refresh
                SaveSite();
            }
        }

        private void SaveSite()
        {

        }
        private void VerifyConnection()
        {

        }

        private void OpenSearchView()
        {
            this.Stage = SiteConnectionWizardStage.Search;
            AdditionMethod = SiteAdditionMethod.Search;
            NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_SearchSitesList_View);
        }

        private void OpenManualEntryView()
        {
            this.Stage = SiteConnectionWizardStage.ManualEntry;
            AdditionMethod = SiteAdditionMethod.Manual;
            NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View);
        }

        private void OpenUsbListView()
        {
            this.Stage = SiteConnectionWizardStage.UsbList;
            AdditionMethod = SiteAdditionMethod.UsbList;
            NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_UsbList_View);
        }
        
        [ImportingConstructor]
        public SiteConnectionWizardViewModel(sconnSite site, IRegionManager regionManager)
        {
            Config = site;
            this._regionManager = regionManager;

            _provider = new SconnClient("",0,"");
            _provider.SiteDiscovered += _provider_SiteDiscovered;

            this.NetworkSites = new ObservableCollection<sconnSite>();
            this.UsbSites = new ObservableCollection<sconnSite>();

            NavigateBackCommand = new DelegateCommand(NavigateBack);
            NavigateForwardCommand = new DelegateCommand(NavigateForward);
            SaveSiteCommand = new DelegateCommand(SaveSite);
            VerifyConnectionCommand = new DelegateCommand(VerifyConnection);

            OpenSearchViewCommand = new DelegateCommand(OpenSearchView);
            OpenManualEntryViewCommand = new DelegateCommand(OpenManualEntryView);
            OpenUsbListViewCommand = new DelegateCommand(OpenUsbListView);
        }


    }


}
