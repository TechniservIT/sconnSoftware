using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{

    public class ipcSiteConfig
    {
        private int _deviceNo = 0;
        private int _evNo = 0;
        private ipcDeviceConfig[] _deviceConfigs;
        private ipcGlobalConfig _globalConfig;
        private ipcEvent[] events;
        private ipcRcpt[] _gsmRcpts;

        public byte[] Hash;

        public byte[] GlobalNameConfig;
        public byte[] NamesHash;

        public void ReloadConfig()
        {
            foreach (var dev in deviceConfigs)
            {
                dev.LoadPropertiesFromConfig();
            }
        }

        public  ipcRcpt[] gsmRcpts
        {
            get
            {
                return _gsmRcpts;
            }
            set
            {
                _gsmRcpts = value;
            }

        }


        public void AddEvent( ipcEvent ipcEv)
        {
            if (ipcEv != null)
            {
                events[_evNo] = ipcEv;
                _evNo++;
            }
        }

        public int deviceNo
        {
            get
            {
                return _deviceNo;
            }
            //set
            //{
            //    if (value > 0) { _deviceNo = value; }
            //}
        }

        public ipcDeviceConfig[] deviceConfigs
        {
            get
            {
                return _deviceConfigs;
            }
            set
            {
                _deviceConfigs = value;
            }
        }

        public ipcGlobalConfig globalConfig
        {
            get
            {
                return _globalConfig;
            }
            set
            {
                _globalConfig = value;
            }
        }

        public ipcSiteConfig()
        {
            _deviceNo = 0;
            _deviceConfigs = new ipcDeviceConfig[deviceNo];
            _globalConfig = new ipcGlobalConfig();
            GlobalNameConfig = new byte[ipcDefines.RAM_NAMES_Global_Total_Size];
            Hash = new byte[ipcDefines.SHA256_DIGEST_SIZE];
            NamesHash = new byte[ipcDefines.SHA256_DIGEST_SIZE];
        }

        public ipcSiteConfig(int devices)
        {
            _deviceNo = devices;
            _deviceConfigs = new ipcDeviceConfig[deviceNo];
            for (int i = 0; i < deviceNo; i++)
            {
                _deviceConfigs[i] = new  ipcDeviceConfig();
            }
            _globalConfig = new  ipcGlobalConfig();
            GlobalNameConfig = new byte[ipcDefines.RAM_NAMES_Global_Total_Size];
            Hash = new byte[ipcDefines.SHA256_DIGEST_SIZE];
            NamesHash = new byte[ipcDefines.SHA256_DIGEST_SIZE];
        }

        public void fillSampleCfg()
        {

        }

        public void addDeviceCfg()
        {
            _deviceNo++;
             ipcDeviceConfig[] nDeviceConfigs = new  ipcDeviceConfig[deviceNo];
            if (_deviceConfigs.GetLength(0) != 0) //copy only if config is not empty
            {
                for (int i = 0; i < deviceNo - 1; i++)
                {
                    nDeviceConfigs[i] = deviceConfigs[i];
                }
            }
            nDeviceConfigs[deviceNo - 1] = new  ipcDeviceConfig();
            _deviceConfigs = nDeviceConfigs;
            globalConfig.memCFG[ipcDefines.mAdrDevNO] = (byte)deviceNo;
        }


    }

}
