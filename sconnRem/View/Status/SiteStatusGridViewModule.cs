using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.View.Menu.ToolTopMenu;
using sconnRem.View.Status.AlarmSystem;
using sconnRem.View.Status.Cctv;
using sconnRem.Wnd.Main;

namespace sconnRem.View.Status
{

    //[ModuleExport(typeof(SiteStatusGridViewModule))]
    //public class SiteStatusGridViewModule : IModule
    //{
    //    [Import]
    //    public IRegionManager RegionManager;

    //    public void Initialize()
    //    {
    //        this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.MainGridContentRegion, typeof(AlarmSystemStatusFullScreen));
    //        this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.MainGridContentRegion, typeof(AlarmSystemStatusMin));
    //        this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.MainGridContentRegion, typeof(CctvStatusStreamingView));
    //    }
    //}

        
    public class SiteStatusGridViewModule
    {
       
    }

}
