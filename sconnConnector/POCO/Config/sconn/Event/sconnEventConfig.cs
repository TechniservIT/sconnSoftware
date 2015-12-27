using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnEventConfig
    {
        public List<sconnEvent> Events { get; set; }

        public sconnEventConfig()
        {
                
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

    }
}
