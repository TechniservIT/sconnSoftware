using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnEventConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnEvent> Events { get; set; }

        public sconnEventConfig()
        {
                Events = new List<sconnEvent>();
        }

        public sconnEventConfig(ipcSiteConfig cfg)
        {
            try
            {
                Events = new List<sconnEvent>();
                if (cfg.events != null)
                {
                    foreach (var ev in cfg.events)
                    {
                        sconnEvent nevent = new sconnEvent(ev.Buffer);
                        Events.Add(nevent);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public byte[] Serialize()
        {
            byte[] Serialized = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
            for (int i = 0; i < Events.Count; i++)
            {
                byte[] partial = Events[i].Serialize();
                partial.CopyTo(Serialized, i * ipcDefines.EVENT_DB_RECORD_LEN);
            }
            return Serialized;
        }

        public void Deserialize(byte[] buffer)
        {
            int relays = buffer[ipcDefines.EVENT_DB_INFO_EVNO_POS];
            for (int i = 0; i < relays; i++)
            {
                byte[] relayCfg = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
                for (int j = 0; j < ipcDefines.EVENT_DB_RECORD_LEN; j++)
                {
                    relayCfg[j] = buffer[ipcDefines.EVENT_DB_ID_POS + i * ipcDefines.EVENT_DB_RECORD_LEN + j];
                }
                sconnEvent relay = new sconnEvent(relayCfg);
                relay.Id = i;
                Events.Add(relay);
            }
        }

        public void Fake()
        {
            sconnEvent zone = new sconnEvent();
            zone.Fake();
            Events.Add(zone);
        }


    }
}
