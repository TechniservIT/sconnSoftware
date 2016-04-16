using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.Abstract.Event;
using sconnConnector.POCO.Config.sconn.IO;
using sconnConnector.POCO.Config.sconn.IO.Output;
using sconnConnector.POCO.Config.sconn.IO.Relay;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnAlarmSystem //: AlarmSystemConfig
    {
        
        public ipcSiteConfig legacySiteConfig { get; set; }

        public sconnAuthorizedDevices AuthorizedDevices { get; set; }
        public sconnDeviceConfig DeviceConfig { get; set; }
        public sconnEventConfig EventConfig { get; set; }
        public sconnGlobalConfig GlobalConfig { get; set; }
        public sconnGsmConfig GsmConfig { get; set; }
        public sconnAlarmZoneConfig ZoneConfig { get; set; }
        public sconnUserConfig UserConfig { get; set; }
        public sconnInputConfig InputConfig { get; set; }
        public sconnOutputConfig OutputConfig { get; set; }
        public sconnRelayConfig RelayConfig { get; set; }

        public sconnAlarmSystem()
        {
            AuthorizedDevices = new sconnAuthorizedDevices();
            DeviceConfig = new sconnDeviceConfig();
            EventConfig = new sconnEventConfig();
            GlobalConfig = new sconnGlobalConfig();
            GsmConfig = new sconnGsmConfig();
            ZoneConfig = new sconnAlarmZoneConfig();
            UserConfig = new sconnUserConfig();
            InputConfig = new sconnInputConfig();
            OutputConfig = new sconnOutputConfig();
            RelayConfig = new sconnRelayConfig();

        }

        public sconnAlarmSystem(ipcSiteConfig cfg) : this()
        {
            legacySiteConfig = cfg;
            ReloadConfig();
        }

        public void ReloadConfig()
        {
            AuthorizedDevices = new sconnAuthorizedDevices(legacySiteConfig);
          //  DeviceConfig = new sconnDeviceConfig(legacySiteConfig);
            EventConfig = new sconnEventConfig(legacySiteConfig);
            GlobalConfig = new sconnGlobalConfig(legacySiteConfig);
            GsmConfig = new sconnGsmConfig(legacySiteConfig);
            ZoneConfig = new sconnAlarmZoneConfig(legacySiteConfig);
            UserConfig =  new sconnUserConfig(legacySiteConfig);

            InputConfig = new sconnInputConfig(legacySiteConfig);
            OutputConfig = new sconnOutputConfig(legacySiteConfig);
            RelayConfig = new sconnRelayConfig(legacySiteConfig);
        }


        public void LoadFake()
        {
            AuthorizedDevices.Fake();
            DeviceConfig.Fake();
            EventConfig.Fake();
            //GlobalConfig.Fake();
            GsmConfig.Fake();
            ZoneConfig.Fake();
            UserConfig.Fake();
            InputConfig.Fake();
            OutputConfig.Fake();
            RelayConfig.Fake();
        }


    }
}
