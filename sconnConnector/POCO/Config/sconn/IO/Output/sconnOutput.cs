using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.IO;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public enum sconnOutputType
    {
        Normal = 1,
        Delayed = 2
    }

    public class sconnOutput : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {

        public int Id { get; set; }
        public sconnOutputType Type { get; set; }
        public int Value { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int NameId { get; set; }

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
            byte[] buffer = new byte[ipcDefines.mAdrOutputMemSize];
            buffer[ipcDefines.mAdrOutputType] = (byte)Type;
            buffer[ipcDefines.mAdrOutputEnabled] = (byte)(Enabled ? 1 : 0);
            buffer[ipcDefines.mAdrOutputVal] = (byte)Value;
            buffer[ipcDefines.mAdrOutputNameAddr] = (byte)NameId;
            return buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Type = (sconnOutputType)buffer[ipcDefines.mAdrOutputMemSize];
            Value = buffer[ipcDefines.mAdrOutputVal];
            NameId = buffer[ipcDefines.mAdrOutputNameAddr];
            Enabled = buffer[ipcDefines.mAdrOutputEnabled] > 0 ? true : false;
        }

        public void Fake()
        {
            this.Id = 0;
            this.Enabled = true;
            this.Name = Guid.NewGuid().ToString();
            this.NameId = 0;
            this.Type = sconnOutputType.Normal;
        }

    }

}
