using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config.sconn.User;

namespace AlarmSystemManagmentService.AlarmSystemUsers
{
    public class AlarmSystemUsersConfigurationService : IAlarmSystemUsersConfigurationService
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnAlarmSystemUserConfig> EntityManager;
        private AlarmSystemConfigManager ConfigManager;

        public AlarmSystemUsersConfigurationService()
        {
            Online = true; //online by default
        }

        public AlarmSystemUsersConfigurationService(AlarmSystemConfigManager man) : this()
        {
            if (man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnAlarmSystemUserConfig>(ConfigManager.Config.AuthorizedDevicesConfig, man.RemoteDevice);
            }

        }

        public bool SaveChanges()
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

        public List<sconnAlarmSystemUser> GetAll()
        {
            if (Online)
            {
                EntityManager.Download();
            }
            return ConfigManager.Config.AlarmUserConfig.Users.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnAlarmSystemUser dev = ConfigManager.Config.AlarmUserConfig.Users.FirstOrDefault(d => d.Id == Id);
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

        public sconnAlarmSystemUser GetById(int Id)
        {
            try
            {
                if (Online)
                {
                    EntityManager.Download();
                }
                sconnAlarmSystemUser dev = ConfigManager.Config.AlarmUserConfig.Users.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Add(sconnAlarmSystemUser device)
        {
            try
            {
                ConfigManager.Config.AlarmUserConfig.Users.Add(device);
                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Update(sconnAlarmSystemUser device)
        {
            try
            {
                ConfigManager.Config.AlarmUserConfig.Users
                   .Where(z => z.Id == device.Id)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.UUID = device.UUID;
                       x.AllowedUntil = device.AllowedUntil;
                       x.Card = device.Card;
                       x.Code = device.Code;
                       x.DomainId = device.DomainId;
                       x.Enabled = device.Enabled;
                       x.Id = device.Id;
                       x.Permissions = device.Permissions;
                       x.Value = device.Value;
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

        public bool Remove(sconnAlarmSystemUser device)
        {
            try
            {
                if (Online)
                {
                    ConfigManager.Config.AlarmUserConfig.Users.Remove(device);
                    return SaveChanges();
                }
                else
                {
                    this.ConfigManager.Config.AlarmUserConfig.Users.Remove(device);
                    return true;
                }

            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }


    }
}
