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
