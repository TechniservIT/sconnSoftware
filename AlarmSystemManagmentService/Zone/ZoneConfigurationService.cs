using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using AlarmSystemManagmentService;
using NLog;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Auth;

namespace AlarmSystemManagmentService
{
    public class ZoneConfigurationService : IZoneConfigurationService
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnAlarmZoneConfig> EntityManager;
        private AlarmSystemConfigManager ConfigManager;

        public ZoneConfigurationService()
        {
            Online = true; //online by default
        }

        public ZoneConfigurationService(AlarmSystemConfigManager man) : this()
        {
            ConfigManager = man;
            EntityManager = new AlarmGenericConfigManager<sconnAlarmZoneConfig>(ConfigManager.Config.ZoneConfig, man.RemoteDevice);
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

        public List<sconnAlarmZone> GetAll()
        {
            EntityManager.Download();
            return ConfigManager.Config.ZoneConfig.Zones.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnAlarmZone dev = this.GetById(Id);
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

        public sconnAlarmZone GetById(int Id)
        {
            try
            {
                EntityManager.Download();
                sconnAlarmZone dev = ConfigManager.Config.ZoneConfig.Zones.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Add(sconnAlarmZone device)
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

        public bool Update(sconnAlarmZone rcpt)
        {
            try
            {
                ConfigManager.Config.ZoneConfig.Zones
                   .Where(z => z.Id == rcpt.Id)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.Enabled = rcpt.Enabled;
                       x.Name = rcpt.Name;
                       x.Type = rcpt.Type;
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

        public bool Remove(sconnAlarmZone device)
        {
            try
            {
                // 'Remove' clears static record instead - replace with new empty record with the same Id
                sconnAlarmZone stub = new sconnAlarmZone { Id = device.Id };
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
