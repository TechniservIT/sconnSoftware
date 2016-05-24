using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.Device;
using sconnConnector.POCO.Config.Abstract.IO;

namespace sconnConnector.POCO.Config.Abstract
{
    public abstract class AlarmSystemDeviceConfig
    {
        public  List<AlarmSystemDevice> Devices { get; set; }
    }

}
