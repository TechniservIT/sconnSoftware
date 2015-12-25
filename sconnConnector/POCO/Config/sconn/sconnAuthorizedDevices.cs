using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public class sconnAuthorizedDevices
    {
        public List<sconnAuthorizedDevice>  Devices { get; set; }

        public sconnAuthorizedDevices()
        {
                
        }

        public sconnAuthorizedDevices(ipcSiteConfig cfg)
        {
                
        }
        
    }
}
