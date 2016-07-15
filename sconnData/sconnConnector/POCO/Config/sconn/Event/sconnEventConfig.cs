using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnEventConfig : IAlarmSystemEntityConfig, IFakeAbleConfiguration
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


        public void Clear()
        {
            this.Events = new List<sconnEvent>();
        }

        public int GetEntityCount()
        {
            return Events.Count;
        }

        public byte[] SerializeEntityWithId(int id)
        {
            try
            {
                return Events[id].Serialize();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }
        public void DeserializeEntityWithId(byte[] buffer)
        {
            try
            {
                Events.Add(new sconnEvent(buffer));
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
