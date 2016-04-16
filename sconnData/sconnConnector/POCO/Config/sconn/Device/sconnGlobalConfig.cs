﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnGlobalConfig : IAlarmSystemNamedConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        private byte[] _memCFG;
        public byte[] memCFG
        {
            get { return _memCFG; }
            set { if (value != null) { _memCFG = value; } }
        }

        public int Id { get; set; }

        public bool Armed { get; set; }
        public bool Violation { get; set; }
        public bool Failure { get; set; }
        public int Devices { get; set; }
        public int Zones { get; set; }

        public string Name { get; set; }
        
        public int Lat { get; set; }
        public int Lng { get; set; }

        public string IpAddress { get; set; }

        public int BusShortMaxDistanceMeters { get; set; }

        public int BusLongMaxDistanceMeters { get; set; }
        
        public bool BusComEnabled { get; set; }

        public bool TcpIpComEnabled { get; set; }

        public byte MaximumPeerSyncIntervalSeconds { get; set; }

        public byte MaximumPeerFailRequests { get; set; }

        public byte BusMaxFailedMessagesPerSecond { get; set; }



        public sconnGlobalConfig()
        {
            _memCFG = new byte[ipcDefines.ipcGlobalConfigSize];
        }

        public sconnGlobalConfig(ipcSiteConfig cfg) :this()
        {
            this.memCFG = cfg.globalConfig.memCFG;
        }


        public byte[] Serialize()
        {
            memCFG[ipcDefines.mAdrArmed] = (byte) (Armed ? 1 : 0);
            memCFG[ipcDefines.mAdrViolation] = (byte)(Violation ? 1 : 0);
            memCFG[ipcDefines.mAdrSysFail] = (byte)(Failure ? 1 : 0);
            memCFG[ipcDefines.mAdrDevNO + 1] = (byte)Devices;
            memCFG[ipcDefines.mAdrZoneNo] = (byte)Zones;
            return memCFG;
        }

        public void Deserialize(byte[] buffer)
        {
            this.memCFG = buffer;
            Armed = memCFG[ipcDefines.mAdrArmed] > 0 ? true : false;
            Violation = memCFG[ipcDefines.mAdrViolation] > 0 ? true : false;
            Failure = memCFG[ipcDefines.mAdrSysFail] > 0 ? true : false;
            Devices = (byte)(memCFG[ipcDefines.mAdrDevNO + 1]);
            Zones = (byte)(memCFG[ipcDefines.mAdrZoneNo]);
        }

        public void Fake()
        {
            
        }

        public byte[] SerializeNames()
        {
           return System.Text.Encoding.BigEndianUnicode.GetBytes(this.Name);
        }

        public void DeserializeNames(byte[] buffer)
        {
            Name = System.Text.Encoding.BigEndianUnicode.GetString(buffer, 0, buffer.GetLength(0));
        }
    }

}
