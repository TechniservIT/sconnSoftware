using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace sconnMobileForms.Service.AlarmSystem.Io
{

    public class AlarmIoRelayConfigService : IAlarmIoConfigService
    {

        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnDeviceConfig> EntityManager;

        public sconnDevice Device { get; set; }
        public sconnRelay Relay { get; set; }

        public AlarmSystemConfigManager ConfigManager;

        public AlarmIoRelayConfigService()
        {
            Online = true; //online by default
        }

        public AlarmIoRelayConfigService(AlarmSystemConfigManager man) : this()
        {
            if (man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice);
            }

        }

        public AlarmIoRelayConfigService(AlarmSystemConfigManager man, sconnDevice device, sconnRelay output) : this()
        {
            if (man != null)
            {
                Device = device;
                Relay = output;
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice, device.Id);
            }
        }

        public void Toggle()
        {
            this.ConfigManager.Config.DeviceConfig.Device.Relays[Relay.Id].Value = (this.ConfigManager.Config.DeviceConfig.Device.Relays[Relay.Id].Value ? true : false);
            SaveChanges();
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return EntityManager.Upload();
            }
            else
            {
                return true;
            }
        }


        public bool Get()
        {
            return this.ConfigManager.Config.DeviceConfig.Device.Relays[Relay.Id].Value;
        }

        public void Set(bool value)
        {
            this.ConfigManager.Config.DeviceConfig.Device.Relays[Relay.Id].Value = value;
            SaveChanges();
        }


    }

}
