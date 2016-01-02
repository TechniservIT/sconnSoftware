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

        public int Type { get; set; }

        public int Value { get; set; }

        public int NameId { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public sconnRelay()
        {

        }

        public sconnRelay(byte[] config, int seqno)
        {
            int baseaddr = ipcDefines.mAdrRelay + ipcDefines.RelayMemSize * seqno;
            Id = seqno;
            Type = config[baseaddr + ipcDefines.mAdrRelayType];
            Value = config[baseaddr + ipcDefines.mAdrRelayVal];
            NameId = config[baseaddr + ipcDefines.mAdrRelayNameAddr];
        }

        public sconnRelay(byte[] config, byte[] namecfg, int seqno)
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
