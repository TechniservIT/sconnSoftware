using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnNetworkingServices.Abstract;
using sconnRem.Navigation;
using SiteManagmentService;

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

    public enum SiteWizardEditMode
    {
        Add,
        Edit
    }



    [Export]
    public class SiteConnectionWizardViewModel : BindableBase
    {
        private sconnSite _Config;
        public sconnSite Config
        {
            get { return _Config; }
            set
            {
               SetProperty(ref _Config, value); 
                //_Config = value;
                //OnPropertyChanged(); 
            }
        }


        private readonly IRegionManager _regionManager;
        private SconnClient _scanSconnClient;
        private ISiteRepository _repository;
        private SconnClient connectionTestClient;

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public SiteConnectionWizardStage Stage { get; set; }
        public SiteAdditionMethod AdditionMethod { get; set; }
        public SiteWizardEditMode EditMode { get; set; }
        
        public ICommand NavigateBackCommand { get; set; }
        public ICommand NavigateForwardCommand { get; set; }

        public ICommand OpenSearchViewCommand { get; set; }
        public ICommand OpenManualEntryViewCommand { get; set; }
        public ICommand OpenUsbListViewCommand { get; set; }

        public ICommand SaveSiteCommand { get; set; }
        public ICommand VerifyConnectionCommand { get; set; }

        public ICommand SelectedSiteChangedCommand { get; set; }

        public ObservableCollection<sconnSite> NetworkSites { get; set; }
        public ObservableCollection<sconnSite> UsbSites { get; set; }

     //   public sconnSite SelectedSite { get; set; }
        public NetworkConnectionState ConnectionState { get; set; }
        public int ConnectionProgressPercentage { get; set; }

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
            this.NetworkSites.Clear();  // = new ObservableCollection<sconnSite>();
            _scanSconnClient.SearchForSite();
        }

        private void TestConnectionToSite()
        {
            if (this.Config != null)
            {
                connectionTestClient = new SconnClient(Config.serverIP, Config.serverPort, Config.authPasswd);
                connectionTestClient.ConnectionStateChanged += Client_ConnectionStateChanged;

                BackgroundWorker bgWorker = new BackgroundWorker();
                bgWorker.DoWork += (s, e) => {
                    connectionTestClient.VerifyConnection();
                };
                bgWorker.RunWorkerCompleted += (s, e) =>
                {

                };
                bgWorker.RunWorkerAsync();

            }
        }

        public void OnSelectedItemChanged(sconnSite site)
        {
            //Config = site;
            if (site != null)
            {
                Config.CopyFrom(site);  //replace default/edited config with selected
            }
        }

        private void Client_ConnectionStateChanged(object sender, ConnectionStateEventArgs e)
        {
            this.ConnectionState = e.State;
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
                Stage == SiteConnectionWizardStage.UsbList)
            {
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_MethodSelection_View);
            }
            else if (Stage == SiteConnectionWizardStage.ManualEntry)
            {
                //find out
                if (AdditionMethod == SiteAdditionMethod.Search)
                {
                    this.Stage = SiteConnectionWizardStage.Search;
                    SearchForSitesInNetwork(); // start network search
                    NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_SearchSitesList_View);
                }
                else if (AdditionMethod == SiteAdditionMethod.Manual)   //back to method selection
                {
                    this.Stage = SiteConnectionWizardStage.MethodSelection;
                    NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_MethodSelection_View);
                }
                else if (AdditionMethod == SiteAdditionMethod.UsbList)
                {
                    this.Stage = SiteConnectionWizardStage.UsbList;
                    NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_UsbList_View);
                }
            }
            else if (Stage == SiteConnectionWizardStage.Test)
            {
                this.Stage = SiteConnectionWizardStage.ManualEntry;
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View);
            }
            else if (Stage == SiteConnectionWizardStage.Summary)
            {
                this.Stage = SiteConnectionWizardStage.Test;
                TestConnectionToSite();
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_Test_View);
            }
        }

        private void NavigateForward()
        {
            if (Stage == SiteConnectionWizardStage.MethodSelection)
            {
                //update stage to site edit - not updated before
                this.Stage = SiteConnectionWizardStage.Test;
                TestConnectionToSite();
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_Test_View);
            }
            else if (Stage == SiteConnectionWizardStage.ManualEntry )
            {
                this.Stage = SiteConnectionWizardStage.Test;
                TestConnectionToSite();
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_Test_View);
            }
            else if (
                Stage == SiteConnectionWizardStage.Search ||
                Stage == SiteConnectionWizardStage.UsbList
                    )
            {
                this.Stage = SiteConnectionWizardStage.ManualEntry; //After selection, show edit for password/verify
                NavigateToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View);
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
            //  Sites = new ObservableCollection<sconnSite>(sconnDataShare.getSites());
          //  sconnDataShare.getSite()
            //_repository.Update(this.Config);
            sconnDataShare.updateSite(this.Config);
            sconnDataShare.Save();
        }

        private void VerifyConnection()
        {

        }

        private void OpenSearchView()
        {
            this.Stage = SiteConnectionWizardStage.Search;
            AdditionMethod = SiteAdditionMethod.Search;
            SearchForSitesInNetwork(); // start network search
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
        public SiteConnectionWizardViewModel(sconnSite site, IRegionManager regionManager, ISiteRepository repository)
        {
            Config = site;
            this._regionManager = regionManager;
            this._repository = repository;

            _scanSconnClient = new SconnClient("",0,"");
            _scanSconnClient.SiteDiscovered += _provider_SiteDiscovered;

            this.NetworkSites = new ObservableCollection<sconnSite>();
            this.UsbSites = new ObservableCollection<sconnSite>();
            BindingOperations.EnableCollectionSynchronization(this.NetworkSites, this);
            BindingOperations.EnableCollectionSynchronization(this.UsbSites, this);
            _scanSconnClient.ScanInit();

            NavigateBackCommand = new DelegateCommand(NavigateBack);
            NavigateForwardCommand = new DelegateCommand(NavigateForward);
            SaveSiteCommand = new DelegateCommand(SaveSite);
            VerifyConnectionCommand = new DelegateCommand(VerifyConnection);

            OpenSearchViewCommand = new DelegateCommand(OpenSearchView);
            OpenManualEntryViewCommand = new DelegateCommand(OpenManualEntryView);
            OpenUsbListViewCommand = new DelegateCommand(OpenUsbListView);

            SelectedSiteChangedCommand = new DelegateCommand<sconnSite>(OnSelectedItemChanged);
        }


    }


}
