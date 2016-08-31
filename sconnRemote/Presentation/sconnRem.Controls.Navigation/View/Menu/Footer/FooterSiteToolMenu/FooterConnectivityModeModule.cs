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
using sconnRem.View.Menu.ToolTopMenu;

namespace sconnRem.Controls.Navigation.View.Menu.Footer.FooterSiteToolMenu
{
    [ModuleExport(typeof(FooterConnectivityModeModule))]
    public class FooterConnectivityModeModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.FooterLeftNavigationRegion, typeof(FooterConnectivityModeView));
        }

    }
}
