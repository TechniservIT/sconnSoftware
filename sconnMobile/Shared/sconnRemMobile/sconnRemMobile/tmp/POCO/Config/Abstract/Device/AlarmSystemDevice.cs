using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.IO;

namespace sconnConnector.POCO.Config.Abstract.Device
{
    public abstract class AlarmSystemDevice 
    {
        public  List<AlarmSystemOutput> Outputs { get; set; }
        public  List<AlarmSystemInput> Inputs { get; set; }
        public  List<AlarmSystemRelay> Relays { get; set; }
    }
}
