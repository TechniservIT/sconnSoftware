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

        public void StoreDeviceConfig(int DevNo)
        {
            mngr.WriteDeviceCfgSingle(site, DevNo);
        }

        public bool ToogleArmStatus()
        {
            try
            {
                if (site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrArmed] == 1)
                {
                    site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrArmed] = (byte)0;
                }
                else
                {
                    site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrArmed] = (byte)1;
                }
                return UploadSiteConfig();
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UploadSiteConfig()
        {
            try
            {
                return mngr.WriteGlobalCfg(site);
            }
            catch (Exception e)
            {   
                return false;
            }
        }


        public int GetDeviceNumber()
        {
            return mngr.getSiteDeviceNo(site);
        }




    }
}
