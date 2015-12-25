using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.Config
{
    public class AlarmSystemConfigManager
    {

        private EndpointInfo info;

        private sconnCfgMngr mngr;
        
        private DeviceCredentials creds;

        public sconnSite site;  //allow direct access to site config

        
        /****** Update interval -  cannot connect to remote device more often then specified    ******/
        public int MinUpdatePeriod { get; set; }
        public DateTime LastUpDateTime { get; set; }

        /****** Configuration of remote alarm system after processing  ********/
        public sconnAlarmSystem Config { get; set; }
        

        public AlarmSystemConfigManager(EndpointInfo endp, DeviceCredentials cred)
        {
            info = endp;
            mngr = new sconnCfgMngr();
            creds = cred;
            MinUpdatePeriod = 500;
            site = new sconnSite("", 500, endp.Hostname, endp.Port, creds.Password);
            this.Config.legacySiteConfig = site.siteCfg;    //link alarm object config to low level registers

        }


        private bool CanUpdateDueToTimingContraints()
        {
            return (DateTime.Now - LastUpDateTime).TotalMilliseconds > MinUpdatePeriod;
        }

        public void LoadSiteConfig()
        {
            if (CanUpdateDueToTimingContraints())
            {
                mngr.ReadSiteRunningConfig(site);
                LastUpDateTime = DateTime.Now;
                site.siteCfg.ReloadConfig();
                LoadAlarmSystemConfig();
            }
        }

        private void LoadAlarmSystemConfig()
        {
            Config.ReloadConfig();
        }

        public void StoreDeviceConfig(int DevNo)
        {
            mngr.WriteDeviceCfgSingle(site, DevNo);
            mngr.WriteDeviceNamesCfgSingle(site, DevNo);
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

        public async Task<bool> UploadSiteConfigAsync()
        {
            try
            {
                bool result = await mngr.WriteGlobalCfgAsync(site);
                return result;
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
