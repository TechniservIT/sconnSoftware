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

        public byte[] Serialize()
        {
            byte[] Serialized = new byte[ipcDefines.ZONE_CFG_TOTAL_LEN];
            Serialized[ipcDefines.mAdrZoneNo_Pos] = (byte)Zones.Count;
            for (int i = 0; i < Zones.Count; i++)
            {
                Serialized[i * ipcDefines.ZONE_CFG_LEN + ipcDefines.ZONE_CFG_ENABLED_POS] = (byte)(Zones[i].Enabled ? 1 : 0);
                Serialized[i * ipcDefines.ZONE_CFG_LEN + ipcDefines.ZONE_CFG_TYPE_POS] = (byte)(Zones[i].Type);
                Serialized[i * ipcDefines.ZONE_CFG_LEN + ipcDefines.ZONE_CFG_NAME_ID_POS] = (byte)(Zones[i].NameId);
            }
            return Serialized;
        }

        public sconnAlarmZoneConfig(ipcSiteConfig cfg) :this()
        {
            int zones = cfg.globalConfig.memCFG[ipcDefines.mAdrZoneNo];
            for (int i = 0; i < zones; i++)
            {
                byte[] zoneCfg = new byte[ipcDefines.ZONE_CFG_LEN];
                for (int j = 0; j < ipcDefines.ZONE_CFG_LEN; j++)
                {
                    zoneCfg[j] = cfg.globalConfig.memCFG[ipcDefines.mAdrZoneCfgStartAddr + i*ipcDefines.ZONE_CFG_LEN];
                }
                sconnAlarmZone zone = new sconnAlarmZone(zoneCfg);
                Zones.Add(zone);
            }
        }
    }

}
