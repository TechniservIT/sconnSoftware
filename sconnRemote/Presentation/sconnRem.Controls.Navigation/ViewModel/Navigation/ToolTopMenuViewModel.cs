using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.Config.Storage;
using sconnConnector.POCO.Config;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.Wnd.Tools;
using SiteManagmentService;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{

    [Export]
    public class ToolTopMenuViewModel : BindableBase
    {
        public sconnSite Site { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }
        private IAlarmSystemNavigationService AlarmNavService { get; set; }
        private ISiteRepository SiteRepository { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        
        public ICommand ShowFileImportCommand { get; set; }
        public ICommand ShowFileExportCommand { get; set; }
        public ICommand ShowGlobalPreferencesCommand { get; set; }
        public ICommand ShowSiteWizardCommand { get; set; }

        private void NavigateMainViewToContract(string contract)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, contract
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
        private void ShowFileImport()
        {
            try
            {
                string fileUri = "";
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".json"; // Default file extension
                dlg.Filter = "sconnConfig (.json)|*.json"; // Filter files by extension

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    fileUri = dlg.FileName;

                    //Serialize site config
                    IAlarmSystemConfigurationStorage storage = new AlarmSystemConfigurationFileStorage();
                    if (storage.IsConfigUriCorrect(fileUri))
                    {
                        // storage.StorageConfigAtUri(SiteNavigationManager.alarmSystemConfigManager, fileUri);
                        AlarmNavService.alarmSystemConfigManager = storage.GetConfigFromUri(fileUri);
                        AlarmNavService.Online = false; //set offline by default when loading from file
                    }

                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }


        }

        private void ShowFileExport()
        {
            try
            {
                //Load write to file window
                string fileUri = "";
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = AlarmNavService.CurrentContextSconnSite.siteName; // Default file name
                dlg.DefaultExt = ".json"; // Default file extension
                dlg.Filter = "sconnConfig (.json)|*.json"; // Filter files by extension

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    fileUri = dlg.FileName;
                    
                    //Serialize site config
                    IAlarmSystemConfigurationStorage storage = new AlarmSystemConfigurationFileStorage();
                    if (storage.IsConfigUriCorrect(fileUri))
                    {
                        storage.StorageConfigAtUri(AlarmNavService.alarmSystemConfigManager, fileUri);
                    }

                }
                
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }


        }

        private void ShowGlobalPreferences()
        {
            WndGlobalPreferences wnd = new WndGlobalPreferences();
            wnd.Show();
        }

        private void ShowSiteWizard()
        {
            // AlarmNavService.OpenSiteWizard();
            SiteRepository.SetCurrentSite(new sconnSite());
            NavigateMainViewToContract(SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View);
        }
        
        private void SetupCmds()
        {
            ShowFileImportCommand = new DelegateCommand(ShowFileImport);
            ShowFileExportCommand = new DelegateCommand(ShowFileExport);
            ShowGlobalPreferencesCommand = new DelegateCommand(ShowGlobalPreferences);
            ShowSiteWizardCommand = new DelegateCommand(ShowSiteWizard);
        }

        public ToolTopMenuViewModel()
        {
            SetupCmds();
        }

        [ImportingConstructor]
        public ToolTopMenuViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService, ISiteRepository repo)
        {
            SiteRepository = repo;
            SetupCmds();
            AlarmNavService = NavService;
            this._regionManager = regionManager;
        }


    }


}
