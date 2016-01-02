using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.IO;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{

    public class sconnOutput : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {

        public int Id { get; set; }
        public int Type { get; set; }
        public int Value { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int NameId { get; set; }

        public sconnOutput()
        {

        }

        public sconnOutput(byte[] config, int seqno)
        {
            int baseaddr = ipcDefines.mAdrOutput + ipcDefines.mAdrOutputMemSize * seqno;
            Id = seqno;
            Type = config[baseaddr + ipcDefines.mAdrOutputType];
            Value = config[baseaddr + ipcDefines.mAdrOutputVal];
            NameId = config[baseaddr + ipcDefines.mAdrOutputNameAddr];
        }

        public sconnOutput(byte[] config, byte[] namecfg, int seqno)
            : this(config, seqno)
        {
            if (namecfg != null)
            {
                Name = System.Text.Encoding.Unicode.GetString(namecfg, 0, namecfg.GetLength(0));
            }


        }


        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        public void Deserialize(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public void Fake()
        {
            throw new NotImplementedException();
        }
    }

}
