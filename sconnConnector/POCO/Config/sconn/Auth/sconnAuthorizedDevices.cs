using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public class sconnAuthorizedDevices : IAlarmSystemConfigurationEntity,ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnAuthorizedDevice>  Devices { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAuthorizedDevices()
        {
            Devices = new List<sconnAuthorizedDevice>();
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] Serialized = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
                for (int i = 0; i < Devices.Count; i++)
                {
                    byte[] partial = Devices[i].Serialize();
                    partial.CopyTo(Serialized, i * ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE);
                }
                return Serialized;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Fake()
        {
            sconnAuthorizedDevice zone = new sconnAuthorizedDevice();
            zone.Fake();
            Devices.Add(zone);
        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                Devices = new List<sconnAuthorizedDevice>();
                if (buffer.Length >= ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE)
                {
                    for (int i = 0; i < ipcDefines.SYS_ALARM_DEV_AUTH_MAX_RECORDS; i++)
                    {
                        byte[] authRecord = new byte[ipcDefines.SYS_ALRM_DEV_AUTH_LEN];
                        for (int j = 0; j < ipcDefines.SYS_ALRM_DEV_AUTH_LEN; j++)
                        {
                            authRecord[j] = buffer[i * ipcDefines.SYS_ALRM_DEV_AUTH_LEN + j];
                        }
                        sconnAuthorizedDevice dev = new sconnAuthorizedDevice(authRecord);
                        if (dev._Serial.Length > 0)
                        {
                            Devices.Add((dev));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public sconnAuthorizedDevices(ipcSiteConfig cfg) :this()
        {
            this.Deserialize(cfg.AuthDevices);
        }
        
    }
}
