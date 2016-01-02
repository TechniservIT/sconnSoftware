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
