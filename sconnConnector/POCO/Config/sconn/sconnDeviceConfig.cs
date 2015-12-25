using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Device;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnDeviceConfig
    {
        public List<AlarmSystemDevice> Devices { get; set; }

        public sconnDeviceConfig()
        {
                this.Devices = new List<AlarmSystemDevice>();
        }
    }

}
