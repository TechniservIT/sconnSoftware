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
using sconnConnector.POCO.Config;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.Wnd.Tools;

namespace sconnRem.Controls.Navigation.ViewModel.Navigation
{
    [Export]
    public class ToolTopMenuViewModel : BindableBase
    {
        public sconnSite Site { get; set; }
        private readonly IRegionManager _regionManager;
        public AlarmSystemConfigManager Manager { get; set; }

        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        public ICommand ShowFileImportCommand { get; set; }
        public ICommand ShowFileExportCommand { get; set; }
        public ICommand ShowGlobalPreferencesCommand { get; set; }
        public ICommand ShowSiteWizardCommand { get; set; }
        

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

        
        private void NavigateToAlarmContract(string contractName)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, contractName
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

        private void NavigateToAlarmUri(Uri uri)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, uri
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


        // TODO separate shells ?
        private void ShowFileImport()
        {
            WndImportConfig wnd = new WndImportConfig();
            wnd.Show();
        }

        private void ShowFileExport()
        {
            WndExportConfig wnd = new WndExportConfig();
            wnd.Show();
        }

        private void ShowGlobalPreferences()
        {
            WndGlobalPreferences wnd = new WndGlobalPreferences();
            wnd.Show();
        }

        private void ShowSiteWizard()
        {
            SiteNavigationManager.OpenSiteWizard();
            //WndSiteWizard wnd = new WndSiteWizard();
            //wnd.Show();
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
        public ToolTopMenuViewModel(IRegionManager regionManager)
        {
            SetupCmds();
            this._regionManager = regionManager;
        }


    }


}
