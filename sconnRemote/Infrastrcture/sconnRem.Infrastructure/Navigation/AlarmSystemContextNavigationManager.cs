using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Device;
using sconnRem.Infrastructure.Content;


namespace sconnRem.Infrastructure.Navigation
{
    public static class AlarmSystemContextNavigationManager
    {
        private static Logger _nlogger = LogManager.GetCurrentClassLogger();

        private static sconnSite currentContextSconnSite;

        //alarm syste,
        private static AlarmSystemConfigManager alarmSystemConfigManager;
        private static CompositionContainer contextContainer;
        private static ComposablePart exportedComposablePart;

        public static  void NavigateRegionManagerToCurrentDeviceContext(IRegionManager manager)
        {
            
        }
        

    }
    
}
