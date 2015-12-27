using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnAlarmZoneConfig
    {
        public List<sconnAlarmZone> Zones { get; set; }

        public sconnAlarmZoneConfig()
        {
                Zones = new List<sconnAlarmZone>();
        }

        public sconnAlarmZoneConfig(ipcSiteConfig cfg) :this()
        {
                
        }
    }

}
