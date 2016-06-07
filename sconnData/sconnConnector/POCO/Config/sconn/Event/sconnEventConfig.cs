using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnEventConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnEvent> Events { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnEventConfig()
        {
                Events = new List<sconnEvent>();
            UUID = Guid.NewGuid().ToString();
        }

        public sconnEventConfig(ipcSiteConfig cfg) : this()
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
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public byte[] Serialize()
        {

            try
            {
                byte[] Serialized = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
                for (int i = 0; i < Events.Count; i++)
                {
                    byte[] partial = Events[i].Serialize();
                    partial.CopyTo(Serialized, i * ipcDefines.EVENT_DB_RECORD_LEN);
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
                int relays = buffer[ipcDefines.EVENT_DB_INFO_EVNO_POS];
                for (int i = 0; i < relays; i++)
                {
                    byte[] relayCfg = new byte[ipcDefines.EVENT_DB_RECORD_LEN];
                    for (int j = 0; j < ipcDefines.EVENT_DB_RECORD_LEN; j++)
                    {
                        relayCfg[j] = buffer[i * ipcDefines.EVENT_DB_RECORD_LEN + j+1]; //offset for ev no - todo - defines
                    }
                    sconnEvent relay = new sconnEvent(relayCfg);
                    relay.Id = i;
                    Events.Add(relay);
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
                sconnEvent zone = new sconnEvent();
                zone.Fake();
                Events.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }


        public string UUID { get; set; }
    }
}
