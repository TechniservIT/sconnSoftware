using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mef;
using Prism.Regions;
using sconnRem.Shells.Config;

namespace sconnRem.Infrastructure.Navigation
{
    public class GlobalShellService : IShellService
    {
        private readonly CompositionContainer _container;
        private readonly IRegionManager _regionManager;

        public GlobalShellService(CompositionContainer container, IRegionManager manager)
        {
            _container = container;
            _regionManager = manager;
        }

        public void ShowShell()
        {
            //var shell = _container.<WndSiteConnectionWizard>();
            //var scoppeedRegion = _regionManager.CreateRegionManager();
            //RegionManager.SetRegionManager(shell, scoppeedRegion);

            //shell.Show(0;)
        }



    }
}
