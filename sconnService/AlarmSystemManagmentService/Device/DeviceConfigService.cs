using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;

namespace AlarmSystemManagmentService
{
    public class DeviceConfigService : IAlarmSystemSingleEntityConfigurationService<sconnDevice>
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnDeviceConfig> EntityManager;

        public AlarmSystemConfigManager ConfigManager;

        public DeviceConfigService()
        {
            Online = true; //online by default
        }


        public DeviceConfigService(AlarmSystemConfigManager man) : this()
        {
            if(man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice);
            }

        }

        public DeviceConfigService(AlarmSystemConfigManager man, int DeviceId) : this()
        {
            if(man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice, DeviceId);
            }

        }

        public bool ToggleOutput(int OutputId)
        {
            if (OutputId <= this.ConfigManager.Config.DeviceConfig.Device.Outputs.Count)
            {
                this.ConfigManager.Config.DeviceConfig.Device.Outputs[OutputId].Value = (
                     this.ConfigManager.Config.DeviceConfig.Device.Outputs[OutputId].Value ? true : false);
                return this.SaveChanges();
            }
            return false;
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

        public sconnDevice Get()
        {
            if (Online)
            {
                EntityManager.Download();
            }
            return ConfigManager.Config.DeviceConfig.Device;
        }

        public bool Update(sconnDevice rcpt)
        {
            try
            {
                ConfigManager.Config.DeviceConfig.Device.Inputs = rcpt.Inputs;
                ConfigManager.Config.DeviceConfig.Device.Outputs = rcpt.Outputs;
                ConfigManager.Config.DeviceConfig.Device.Relays = rcpt.Relays;
                ConfigManager.Config.DeviceConfig.Device.Name = rcpt.Name;
                ConfigManager.Config.DeviceConfig.Device.NetworkConfig = rcpt.NetworkConfig;
                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Remove(sconnDevice device)
        {
            try
            {
                // 'Remove' clears static record instead - replace with new empty record with the same Id
                sconnDevice stub = new sconnDevice { Id = device.Id };
                this.Update(stub);
                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

    }
}
