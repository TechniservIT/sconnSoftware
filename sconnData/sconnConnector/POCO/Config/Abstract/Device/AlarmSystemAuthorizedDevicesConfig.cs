using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract
{
    public abstract class AlarmSystemAuthorizedDevicesConfig
    {
        public abstract List<AlarmSystemAuthorizedDevice> Devices { get; set; }
    }
}
