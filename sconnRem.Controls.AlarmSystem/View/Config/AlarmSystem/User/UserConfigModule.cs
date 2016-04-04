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
using sconnRem.View.Config.AlarmSystem.Zone;

namespace sconnRem.View.Config.AlarmSystem.User
{
    [ModuleExport(typeof(UserConfigModule))]
    public class UserConfigModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(AlarmRegionNames.MainNavigationRegion, typeof(UserConfigViewNavigationItem));
        }
    }
}
