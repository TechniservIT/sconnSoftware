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
            Devices = new List<sconnAuthorizedDevice>();
        }

        public byte[] Serialize()
        {
            byte[] bytes = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
            for (int i = 0; i < Devices.Count; i++)
            {
                sconnAuthorizedDevice dev = Devices[i];
                char[] uuidBytes = dev._Serial.ToString().ToCharArray();
                for (int j = 0; j < uuidBytes.Length; j++)
                {
                    bytes[i * ipcDefines.SYS_ALRM_UUID_LEN + j] = (byte)uuidBytes[j];
                }
            }
            return bytes;
        }

        void DeSerialize(byte[] buffer)
        {
            Devices = new List<sconnAuthorizedDevice>();
            for (int i = 0; i < ipcDefines.SYS_ALARM_DEV_AUTH_MAX_RECORDS; i++)
            {
                try
                {
                    sconnAuthorizedDevice dev = new sconnAuthorizedDevice();
                    string uuid;
                    // TODO date range
                    // TODO enable
                    byte[] uuidBytes = new byte[ipcDefines.SYS_ALRM_UUID_LEN];
                    for (int j = 0; j < ipcDefines.SYS_ALRM_UUID_LEN; j++)
                    {
                        uuidBytes[j] = (byte)buffer[i * ipcDefines.SYS_ALRM_UUID_LEN + j];
                    }
                    uuid = Encoding.Unicode.GetString(uuidBytes);
                    if (uuid.Length != 0)
                    {
                        dev._Serial = uuid;
                        this.Devices.Add(dev);
                    }
                }
                catch (Exception)
                {
                        
                }

            }
        }

        public sconnAuthorizedDevices(ipcSiteConfig cfg) :this()
        {
            
        }
        
    }
}
