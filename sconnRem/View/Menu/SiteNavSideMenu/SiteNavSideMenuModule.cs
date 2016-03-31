using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.View.Config.AlarmSystem.Auth;
using sconnRem.Wnd.Main;

namespace sconnRem.View.Menu.SiteNavSideMenu
{
    [ModuleExport(typeof(SiteNavSideMenuModule))]
    public class SiteNavSideMenuModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.RNavigationRegion, typeof(SiteNavSideMenuView));
        }
    }
    
}
