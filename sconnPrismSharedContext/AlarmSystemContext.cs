using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config;

namespace sconnPrismSharedContext
{

    public static class AlarmSystemContext
    {
        private static AlarmSystemConfigManager alarmSystemConfigManager;

        public static void SetManager(AlarmSystemConfigManager manager)
        {
            AlarmSystemContext.alarmSystemConfigManager = manager;
        }

        public static AlarmSystemConfigManager GetManager()
        {
            return alarmSystemConfigManager;
        }

    }
    
}
