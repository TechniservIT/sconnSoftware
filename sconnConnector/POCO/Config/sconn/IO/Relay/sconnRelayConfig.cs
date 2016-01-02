using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn.IO.Relay
{
    public class sconnRelayConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnRelay> Relays { get; set; }

        public sconnRelayConfig()
        {
            Relays = new List<sconnRelay>();
        }

        public sconnRelayConfig(ipcSiteConfig cfg) : this()
        {

        }

        public byte[] Serialize()
        {
            byte[] Serialized = new byte[ipcDefines.RelayTotalMemSize];
            for (int i = 0; i < Relays.Count; i++)
            {
                byte[] partial = Relays[i].Serialize();
                partial.CopyTo(Serialized,i*ipcDefines.RelayMemSize);
            }
            return Serialized;
        }

        public void Deserialize(byte[] buffer)
        {
            int relays = buffer[ipcDefines.DeviceMaxRelays];
            for (int i = 0; i < relays; i++)
            {
                byte[] relayCfg = new byte[ipcDefines.RelayMemSize];
                for (int j = 0; j < ipcDefines.ZONE_CFG_LEN; j++)
                {
                    relayCfg[j] = buffer[ipcDefines.mAdrZoneCfgStartAddr + i * ipcDefines.ZONE_CFG_LEN];
                }
                sconnRelay relay = new sconnRelay(relayCfg);
                relay.Id = i;
                Relays.Add(relay);
            }
        }

        public void Fake()
        {
            sconnRelay zone = new sconnRelay();
            zone.Fake();
            Relays.Add(zone);
        }
    }
}
