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
using sconnRem.View.Config.AlarmSystem.Gsm;

namespace sconnRem.Controls.AlarmSystem.View.Status.Comm
{
    [ModuleExport(typeof(AlarmSystemCommStatusViewModule))]
    public class AlarmSystemCommStatusViewModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(AlarmRegionNames.MainContentRegion, typeof(AlarmSystemCommStatusView));
        }
    }
    
}
