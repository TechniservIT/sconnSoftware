using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract.IO;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public enum sconnOutputType
    {
        AlarmNormallyActive = 0,
        AlarmNormallyInActive,
        Power,
        SignalNormallyActive,
        SignalNormallyInactive,
        OnFailure,
        OnViolation,
        OnGsmTransmission,
        OnConfigUpload
    }
    
    public class sconnOutput : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {

        public int Id { get; set; }
        public sconnOutputType Type { get; set; }
        public int Value { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int NameId { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnOutput() 
        {
            Name = "Output";
        }
         
        public sconnOutput(byte[] rawCfg) : this()
        {
            this.Deserialize(rawCfg);
        }


        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.mAdrOutputMemSize];
                buffer[ipcDefines.mAdrOutputType] = (byte)Type;
                buffer[ipcDefines.mAdrOutputEnabled] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.mAdrOutputVal] = (byte)Value;
                buffer[ipcDefines.mAdrOutputNameAddr] = (byte)NameId;
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
                Type = (sconnOutputType)buffer[ipcDefines.mAdrOutputType];
                Value = buffer[ipcDefines.mAdrOutputVal];
                NameId = buffer[ipcDefines.mAdrOutputNameAddr];
                Enabled = buffer[ipcDefines.mAdrOutputEnabled] > 0 ? true : false;
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
