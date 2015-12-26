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

        public sconnEventConfig(ipcSiteConfig cfg)
        {

        }

    }
}
