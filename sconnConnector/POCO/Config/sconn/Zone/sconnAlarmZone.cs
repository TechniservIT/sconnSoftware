using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn
{
    public enum AlarmZoneType
    {
        General = 1,
        Nightly,
        Z24h,
        IoControl,
        Time_Guarded
    }

    public class sconnAlarmZone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AlarmZoneType Type { get; set; }
        public bool Enabled { get; set; }
        public int NameId { get; set; }

        public sconnAlarmZone()
        {
                
        }

        public sconnAlarmZone(byte[] serialized) : this()
        {
                //todo - load name
            Type = (AlarmZoneType)serialized[ipcDefines.ZONE_CFG_TYPE_POS];
            Enabled = serialized[ipcDefines.ZONE_CFG_ENABLED_POS] > 0 ? true  : false;
        }

        public byte[] Serialize()
        {
            //TODO
            return new byte[0];
        }

    }

}
