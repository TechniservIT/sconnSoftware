using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn.IO.Output
{
    public class sconnOutputConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnOutput> Outputs { get; set; }

        public sconnOutputConfig()
        {
            Outputs = new List<sconnOutput>();
        }

        public byte[] Serialize()
        {
            byte[] Serialized = new byte[ipcDefines.OutputsTotalMemSize];
            for (int i = 0; i < Outputs.Count; i++)
            {
                byte[] partial = Outputs[i].Serialize();
                partial.CopyTo(Serialized, i * ipcDefines.mAdrOutputMemSize);
            }
            return Serialized;
        }

        public void Deserialize(byte[] buffer)
        {
            int outputs = buffer[ipcDefines.DeviceMaxOutputs];
            for (int i = 0; i < outputs; i++)
            {
                byte[] relayCfg = new byte[ipcDefines.mAdrOutputMemSize];
                for (int j = 0; j < ipcDefines.mAdrOutputMemSize; j++)
                {
                    relayCfg[j] = buffer[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize];
                }
                sconnOutput relay = new sconnOutput(relayCfg);
                relay.Id = i;
                Outputs.Add(relay);
            }
        }

        public void Fake()
        {
            sconnOutput zone = new sconnOutput();
            zone.Fake();
            Outputs.Add(zone);
        }

    }
}
