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
    public class DeviceConfigService : IAlarmSystemConfigurationService<sconnDevice>
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
            ConfigManager = man;
            EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.UserConfig, man.RemoteDevice);
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

        public List<sconnDevice> GetAll()
        {
            EntityManager.Download();
            return ConfigManager.Config.DeviceConfig.Devices.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnDevice dev = this.GetById(Id);
                if (dev != null)
                {
                    return this.Remove(dev);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public sconnDevice GetById(int Id)
        {
            try
            {
                EntityManager.Download();
                sconnDevice dev = ConfigManager.Config.DeviceConfig.Devices.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Add(sconnDevice device)
        {
            try
            {
                return true;    //no adding -  filled with empty objects
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Update(sconnDevice rcpt)
        {
            try
            {
                ConfigManager.Config.DeviceConfig.Devices
                   .Where(z => z.Id == rcpt.Id)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.Inputs = rcpt.Inputs;
                       x.Outputs = rcpt.Outputs;
                       x.Relays = rcpt.Relays;
                       x.Name = rcpt.Name;
                       x.NetworkConfig = rcpt.NetworkConfig;
                   }
                   );
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
