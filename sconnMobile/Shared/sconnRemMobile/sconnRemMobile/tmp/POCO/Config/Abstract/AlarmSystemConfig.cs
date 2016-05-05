using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Event;

namespace sconnConnector.POCO.Config
{
    public abstract class AlarmSystemConfig
    {
        protected abstract AlarmSystemAuthorizedDevicesConfig AuthorizedDevices { get; set; }

        protected abstract  AlarmSystemGsmConfig GsmConfig { get; set; }

        protected abstract AlarmSystemZoneConfig ZoneConfig { get; set; }

        protected abstract  AlarmSystemDeviceConfig DeviceConfigs { get; set; }

        protected abstract  AlarmSystemGlobalConfig GlobalConfig  { get; set; }

        protected abstract AlarmSystemEventConfig EventConfig { get; set; }
    }


}
