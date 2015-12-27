using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnUserConfig
    {
        public List<sconnUser> Users { get; set; }

        public sconnUserConfig()
        {
                Users = new List<sconnUser>();
        }

        public sconnUserConfig(ipcSiteConfig cfg) :this()
        {
                
        }

    }
}
