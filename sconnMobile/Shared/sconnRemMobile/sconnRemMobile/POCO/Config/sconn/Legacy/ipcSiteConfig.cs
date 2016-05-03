﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{

    public class ipcSiteConfig
    {
        public int _deviceNo = 0;
        public int _evNo = 0;
        public ipcDeviceConfig[] deviceConfigs;
        public ipcGlobalConfig globalConfig;
        public ipcEvent[] events;
        
        public bool Violation;
        public bool Armed;

        public byte[] GsmConfig;
        public byte[] AuthDevices;
        public byte[] UserConfig;
        public byte[] Hash;
        public byte[] GlobalNameConfig;
        public byte[] NamesHash;

        public int ZoneNumber { get; set; }
        public byte[][] ZoneNames { get; set; }

        public void ReloadConfig()
        {
            foreach (var dev in deviceConfigs)
            {
                dev.LoadPropertiesFromConfig();
            }
            this.Violation = globalConfig.memCFG[ipcDefines.mAdrViolation] > 0 ? true : false;
            this.Armed = globalConfig.memCFG[ipcDefines.mAdrArmed] > 0 ? true : false;
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
        }
        

        public ipcSiteConfig()
        {
            _deviceNo = 0;
            deviceConfigs = new ipcDeviceConfig[deviceNo];
            globalConfig = new ipcGlobalConfig();
            GsmConfig = new byte[0];
            AuthDevices = new byte[0];
            UserConfig = new byte[0];
            events = new ipcEvent[0];
            ZoneNames = new byte[0][];
            GlobalNameConfig = new byte[ipcDefines.RAM_NAMES_Global_Total_Size];
            Hash = new byte[ipcDefines.SHA256_DIGEST_SIZE];
            NamesHash = new byte[ipcDefines.SHA256_DIGEST_SIZE];
        }

        public ipcSiteConfig(int devices) : this()
        {
            _deviceNo = devices;
            deviceConfigs = new ipcDeviceConfig[deviceNo];
            for (int i = 0; i < deviceNo; i++)
            {
                deviceConfigs[i] = new  ipcDeviceConfig();
            }
        }

       
        public void addDeviceCfg()
        {
            _deviceNo++;
             ipcDeviceConfig[] nDeviceConfigs = new  ipcDeviceConfig[deviceNo];
            if (deviceConfigs.GetLength(0) != 0) //copy only if config is not empty
            {
                for (int i = 0; i < deviceNo - 1; i++)
                {
                    nDeviceConfigs[i] = deviceConfigs[i];
                }
            }
            nDeviceConfigs[deviceNo - 1] = new  ipcDeviceConfig();
            deviceConfigs = nDeviceConfigs;
            globalConfig.memCFG[ipcDefines.mAdrDevNO] = (byte)deviceNo;
        }


    }

}
