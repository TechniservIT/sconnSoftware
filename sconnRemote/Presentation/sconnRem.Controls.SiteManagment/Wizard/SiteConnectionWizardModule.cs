using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Navigation;

namespace sconnRem.Controls.SiteManagment.Wizard
{

    [ModuleExport(typeof(SiteConnectionWizardModule))]
    public class SiteConnectionWizardModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(SiteManagmentRegionNames.MainContentRegion, typeof(SiteConnectionWizardManualEntryView));
            this.RegionManager.RegisterViewWithRegion(SiteManagmentRegionNames.MainContentRegion, typeof(SiteConnectionWizardMethodSelectionView));
            this.RegionManager.RegisterViewWithRegion(SiteManagmentRegionNames.MainContentRegion, typeof(SiteConnectionWizardSearchSitesListView));
            this.RegionManager.RegisterViewWithRegion(SiteManagmentRegionNames.MainContentRegion, typeof(SiteConnectionWizardSummaryView));
            this.RegionManager.RegisterViewWithRegion(SiteManagmentRegionNames.MainContentRegion, typeof(SiteConnectionWizardTestView));
            this.RegionManager.RegisterViewWithRegion(SiteManagmentRegionNames.MainContentRegion, typeof(SiteConnectionWizardUsbListView));
        }
    }
    
}
