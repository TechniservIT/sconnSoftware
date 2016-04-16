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
using sconnRem.View.Menu.GridNavSideMenu;

namespace sconnRem.Controls.Navigation.View.Menu.ContextToolbar.AlarmSystem
{

    [ModuleExport(typeof(AlarmSystemToolbarModule))]
    public class AlarmSystemToolbarModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.TopContextToolbarRegion, typeof(AlarmSystemToolbarView));
        }
    }
    
}
