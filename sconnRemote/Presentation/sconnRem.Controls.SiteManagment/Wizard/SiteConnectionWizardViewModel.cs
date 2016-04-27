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
using sconnConnector.POCO.Config.sconn;

namespace sconnRem.Controls.SiteManagment.Wizard
{

    [Export]
    public class SiteConnectionWizardViewModel : BindableBase
    {
        public sconnSite Config { get; set; }
        private readonly IRegionManager _regionManager;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        
        private ICommand NavigateBackCommand;
        private ICommand NavigateForwardCommand;

        private ICommand SaveSiteCommand;
        private ICommand VerifyConnectionCommand;

        private void NavigateBack()
        {
            
        }

        private void NavigateForward()
        {

        }

        private void SaveSite()
        {

        }
        private void VerifyConnection()
        {

        }

        //[ImportingConstructor]
        //public SiteConnectionWizardViewModel( IRegionManager regionManager)
        //{
        //    Config = new sconnSite();
        //    this._regionManager = regionManager;

        //    NavigateBackCommand = new DelegateCommand(NavigateBack);
        //    NavigateForwardCommand = new DelegateCommand(NavigateForward);
        //    SaveSiteCommand = new DelegateCommand(SaveSite);
        //    VerifyConnectionCommand = new DelegateCommand(VerifyConnection);
        //}

        [ImportingConstructor]
        public SiteConnectionWizardViewModel(sconnSite site, IRegionManager regionManager)
        {
            Config = site;
            this._regionManager = regionManager;

            NavigateBackCommand = new DelegateCommand(NavigateBack);
            NavigateForwardCommand = new DelegateCommand(NavigateForward);
            SaveSiteCommand = new DelegateCommand(SaveSite);
            VerifyConnectionCommand = new DelegateCommand(VerifyConnection);
        }


    }


}
