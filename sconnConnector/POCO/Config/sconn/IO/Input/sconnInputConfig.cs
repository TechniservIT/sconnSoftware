using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn.IO
{
    public class sconnInputConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {

        public List<sconnInput> Inputs { get; set; }

        public sconnInputConfig()
        {
            Inputs = new List<sconnInput>();
        }
        public byte[] Serialize()
        {
            byte[] Serialized = new byte[ipcDefines.mAdrInputMemSize];
            for (int i = 0; i < Inputs.Count; i++)
            {
                byte[] partial = Inputs[i].Serialize();
                partial.CopyTo(Serialized, i * ipcDefines.mAdrInputMemSize);
            }
            return Serialized;
        }

        public void Deserialize(byte[] buffer)
        {
            int relays = buffer[ipcDefines.DeviceMaxInputs];
            for (int i = 0; i < relays; i++)
            {
                byte[] relayCfg = new byte[ipcDefines.mAdrInputMemSize];
                for (int j = 0; j < ipcDefines.mAdrInputMemSize; j++)
                {
                    relayCfg[j] = buffer[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize];
                }
                sconnInput relay = new sconnInput(relayCfg);
                relay.Id = i;
                Inputs.Add(relay);
            }
        }

        public void Fake()
        {
            sconnInput zone = new sconnInput();
            zone.Fake();
            Inputs.Add(zone);
        }

    }
}
