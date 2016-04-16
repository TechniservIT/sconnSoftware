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

namespace sconnRem.View.Config.AlarmSystem.Gsm
{
    [ModuleExport(typeof(GsmConfigModule))]
    public class GsmConfigModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(AlarmRegionNames.MainNavigationRegion, typeof(GsmConfigViewNavigationItem));
        }
    }
}
