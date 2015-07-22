using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.Config
{
    public class AlarmSystemConfigManager
    {

        private EndpointInfo info;

        private sconnCfgMngr mngr;

        public sconnSite site;

        private DeviceCredentials creds;

        public AlarmSystemConfigManager(EndpointInfo endp, DeviceCredentials cred)
        {
            info = endp;
            mngr = new sconnCfgMngr();
            creds = cred;
            site = new sconnSite("", 500, endp.Hostname, endp.Port, creds.Password);
            
        }

        public void LoadSiteConfig()
        {
            mngr.ReadSiteRunningConfig(site);
        }



        public int GetDeviceNumber()
        {
            return mngr.getSiteDeviceNo(site);
        }




    }
}
