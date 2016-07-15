using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.Abstract.Event;

namespace sconnConnector.POCO.Config.sconn
{


    public class sconnAlarmSystem :BindableBase
    {
        public Device.Device RemoteDevice { get; set; }

        private ipcSiteConfig _legacySiteConfig;
        public ipcSiteConfig legacySiteConfig
        {
            get
            {
                return _legacySiteConfig;;
            }
            set
            {
                _legacySiteConfig = value;
                OnPropertyChanged();
            } 
        }
        
        public sconnAuthorizedDevicesConfig AuthorizedDevicesConfig { get; set; }

        public sconnDeviceConfig DeviceConfig { get; set; }

        public sconnEventConfig EventConfig { get; set; }

        public sconnGlobalConfig GlobalConfig { get; set; }

        public sconnGsmConfig GsmConfig { get; set; }

        public sconnAlarmZoneConfig ZoneConfig { get; set; }

        public sconnUserConfig UserConfig { get; set; }


        public sconnAlarmSystem()
        {
            AuthorizedDevicesConfig = new sconnAuthorizedDevicesConfig();
            DeviceConfig = new sconnDeviceConfig();
            EventConfig = new sconnEventConfig();
            GlobalConfig = new sconnGlobalConfig();
            GsmConfig = new sconnGsmConfig();
            ZoneConfig = new sconnAlarmZoneConfig();
            UserConfig = new sconnUserConfig();

            
        }
        

        public void LoadFake()
        {
            AuthorizedDevicesConfig.Fake();
            DeviceConfig.Fake();
            EventConfig.Fake();
            GsmConfig.Fake();
            ZoneConfig.Fake();
            UserConfig.Fake();
        }


    }
}
