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
    public class AlarmIoOutputConfigService : IAlarmIoConfigService
    {

        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnDeviceConfig> EntityManager;

        public sconnDevice Device { get; set; }
        public sconnOutput Output { get; set; }

        public AlarmSystemConfigManager ConfigManager;

        public AlarmIoOutputConfigService()
        {
            Online = true; //online by default
        }
        
        public AlarmIoOutputConfigService(AlarmSystemConfigManager man) : this()
        {
            if (man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice);
            }

        }

        public AlarmIoOutputConfigService(AlarmSystemConfigManager man, sconnDevice device, sconnOutput output) : this()
        {
            if (man != null)
            {
                Device = device;
                Output = output;
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice, device.Id);
            }
        }
        
        public void Toggle()
        {
            this.ConfigManager.Config.DeviceConfig.Device.Outputs[Output.Id].Value = (this.ConfigManager.Config.DeviceConfig.Device.Outputs[Output.Id].Value ? true : false);
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
            return this.ConfigManager.Config.DeviceConfig.Device.Outputs[Output.Id].Value;
        }

        public void Set(bool value)
        {
            this.ConfigManager.Config.DeviceConfig.Device.Outputs[Output.Id].Value =value;
            SaveChanges();
        }
    }
}
