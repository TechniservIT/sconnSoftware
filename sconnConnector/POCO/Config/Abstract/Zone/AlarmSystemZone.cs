using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract
{
    public enum AlarmSystemZoneType
    {
       General = 1,
        Nightly,
        Z24h,
        IoControl,
        Time_Guarded
    }

    public abstract class AlarmSystemZone
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public AlarmSystemZoneType Type { get; set; }
    }
}
