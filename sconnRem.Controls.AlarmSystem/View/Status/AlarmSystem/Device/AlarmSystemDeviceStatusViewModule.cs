using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.View.Status.Comm;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.View.Status.Device
{
    [ModuleExport(typeof(AlarmSystemDeviceStatusViewModule))]
    public class AlarmSystemDeviceStatusViewModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(AlarmRegionNames.MainContentRegion, typeof(AlarmSystemDeviceStatusView));
        }
    }
   
}
