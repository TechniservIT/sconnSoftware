using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnAlarmZoneConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
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

        public void LoadNames(byte[][] NamesBf)
        {
                for (int i = 0; i < ipcDefines.ZONE_CFG_MAX_ZONES; i++)
                {
                    try
                    {
                        string name = Encoding.BigEndianUnicode.GetString(NamesBf[i]);
                        this.Zones[i].Name = name;
                    }
                    catch (Exception e)
                    {
                    }

                }
        }

        public void Deserialize(byte[] buffer)
        {
            int zones = ipcDefines.ZONE_CFG_MAX_ZONES;  //buffer[ipcDefines.mAdrZoneNo_Pos];  //ipcDefines.ZONE_CFG_MAX_ZONES;  
            for (int i = 0; i < zones; i++)
            {
                byte[] zoneCfg = new byte[ipcDefines.ZONE_CFG_LEN];
                for (int j = 0; j < ipcDefines.ZONE_CFG_LEN; j++)
                {
                    zoneCfg[j] = buffer[ipcDefines.mAdrZoneCfgStartAddr + i * ipcDefines.ZONE_CFG_LEN +j];
                }
                sconnAlarmZone zone = new sconnAlarmZone(zoneCfg);
                zone.Id = i;
                Zones.Add(zone);
            }
        }

        public sconnAlarmZoneConfig(ipcSiteConfig cfg) :this()
        {
            this.Deserialize(cfg.globalConfig.memCFG);
            this.LoadNames(cfg.ZoneNames);
        }

        public void Fake()
        {
            sconnAlarmZone zone = new sconnAlarmZone();
            zone.Fake();
            Zones.Add(zone);
        }
    }

}
