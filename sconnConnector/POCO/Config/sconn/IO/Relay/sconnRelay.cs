using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.sconn;
using iotDbConnector.DAL;

namespace sconnConnector.POCO.Config
{



    public class sconnRelay : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public byte Id { get; set; }
        public sconnOutputType Type { get; set; }
        public bool Value { get; set; }
        public byte NameId { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public DeviceIoCategory IoCategory { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnRelay()
        {
            Name = "Relay";
            IoCategory = DeviceIoCategory.Relay; 
        }

        public sconnRelay(byte[] rawCfg) : this()
        {
                this.Deserialize(rawCfg);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.RelayMemSize];
                buffer[ipcDefines.mAdrRelayType] = (byte)Type;
                buffer[ipcDefines.mAdrRelayEnabled] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.mAdrRelayVal] = (byte)(Value ? 1 : 0);
                buffer[ipcDefines.mAdrRelayNameAddr] = (byte)NameId;
                return buffer;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                Type = (sconnOutputType)buffer[ipcDefines.mAdrRelayType];
                Value = buffer[ipcDefines.mAdrRelayVal] > 0 ? true : false;
                NameId = buffer[ipcDefines.mAdrRelayNameAddr];
                Enabled = buffer[ipcDefines.mAdrRelayEnabled] > 0 ? true : false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public void Fake()
        {
            try
            {
                this.Id = 0;
                this.Enabled = true;
                this.Name = Guid.NewGuid().ToString();
                this.NameId = 0;
                this.Type = sconnOutputType.AlarmNormallyActive;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }
    }


}
