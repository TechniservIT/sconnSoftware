using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.sconn.IO.Relay
{
    public class sconnRelayConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnRelay> Relays { get; set; }

        public sconnRelayConfig()
        {
            Relays = new List<sconnRelay>();
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
