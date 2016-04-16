﻿using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.View.Config.AlarmSystem.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnRem.Navigation;

namespace sconnRem.View.Config.AlarmSystem.Auth
{
    [ModuleExport(typeof(AlarmAuthConfigModule))]
    public class AlarmAuthConfigModule : IModule
    {
        [Import]
        public IRegionManager RegionManager;

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(AlarmRegionNames.MainNavigationRegion, typeof(AuthConfigViewNavigationItem));
        }
    }

}
