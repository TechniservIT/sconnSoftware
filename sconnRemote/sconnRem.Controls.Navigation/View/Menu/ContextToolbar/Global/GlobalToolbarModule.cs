using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Controls.Navigation.View.Menu.ContextToolbar.AlarmSystem;
using sconnRem.Navigation;

namespace sconnRem.Controls.Navigation.View.Menu.ContextToolbar.Global
{
    [ModuleExport(typeof(GlobalToolbarModule))]
    public class GlobalToolbarModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.TopContextToolbarRegion, typeof(GlobalToolbarView));
        }
    }
}
