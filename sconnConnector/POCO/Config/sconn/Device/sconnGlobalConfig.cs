using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnGlobalConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        private byte[] _memCFG;
        public byte[] memCFG
        {
            get { return _memCFG; }
            set { if (value != null) { _memCFG = value; } }
        }

        public int Id { get; set; }

        public bool Armed { get; set; }
        public bool Violation { get; set; }
        public int Devices { get; set; }

        public int Lat { get; set; }
        public int Lng { get; set; }



        public sconnGlobalConfig()
        {
            _memCFG = new byte[ipcDefines.ipcGlobalConfigSize];
        }

        public sconnGlobalConfig(ipcSiteConfig cfg) :this()
        {
            this.memCFG = cfg.globalConfig.memCFG;
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
