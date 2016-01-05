using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sconnConnector.POCO.Config.sconn.IO.Relay
{
    public class sconnRelayConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnRelay> Relays { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnRelayConfig()
        {
            Relays = new List<sconnRelay>();
        }

        public sconnRelayConfig(ipcSiteConfig cfg) : this()
        {

        }

        public byte[] Serialize()
        {
            try
            {
                byte[] Serialized = new byte[ipcDefines.RelayTotalMemSize];
                for (int i = 0; i < Relays.Count; i++)
                {
                    byte[] partial = Relays[i].Serialize();
                    partial.CopyTo(Serialized, i * ipcDefines.RelayMemSize);
                }
                return Serialized;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Deserialize(byte[] buffer)
        {
            try
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
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public void Fake()
        {
            try
            {
                sconnRelay zone = new sconnRelay();
                zone.Fake();
                Relays.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }
    }
}
