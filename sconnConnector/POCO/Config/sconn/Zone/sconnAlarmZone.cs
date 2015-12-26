using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn
{
    public enum AlarmZoneType
    {
        
    }

    public class sconnAlarmZone
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public AlarmZoneType Type { get; set; }
    }

}
