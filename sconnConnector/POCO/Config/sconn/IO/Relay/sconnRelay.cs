using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{



    public class sconnRelay : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public int Id { get; set; }

        public sconnOutputType Type { get; set; }

        public int Value { get; set; }

        public int NameId { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public sconnRelay()
        {

        }

        public sconnRelay(byte[] rawCfg)
        {
                this.Deserialize(rawCfg);
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[ipcDefines.RelayMemSize];
            buffer[ipcDefines.mAdrRelayType] = (byte)Type;
            buffer[ipcDefines.mAdrRelayEnabled] = (byte)(Enabled ? 1 : 0);
            buffer[ipcDefines.mAdrRelayVal] = (byte)Value;
            buffer[ipcDefines.mAdrRelayNameAddr] = (byte) NameId;
            return buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Type = (sconnOutputType)buffer[ipcDefines.mAdrRelayType];
            Value = buffer[ipcDefines.mAdrRelayVal];
            NameId = buffer[ipcDefines.mAdrRelayNameAddr];
            Enabled = buffer[ipcDefines.mAdrRelayEnabled] > 0 ? true : false;
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
