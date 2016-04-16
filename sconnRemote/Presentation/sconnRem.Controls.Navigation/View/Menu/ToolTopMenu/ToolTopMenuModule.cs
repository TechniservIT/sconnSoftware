﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Navigation;

namespace sconnRem.View.Menu.ToolTopMenu
{

    [ModuleExport(typeof(ToolTopMenuModule))]
    public class ToolTopMenuModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(GlobalViewRegionNames.RopNavigationRegion, typeof(ToolTopMenuView));
        }
    }
    
}
