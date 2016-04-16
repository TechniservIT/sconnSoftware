using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService
{
    public class UsersConfigurationService : IUsersConfigurationService
    {

        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnUserConfig> EntityManager;
        private AlarmSystemConfigManager ConfigManager;

        public UsersConfigurationService()
        {
            Online = true; //online by default
        }

        public UsersConfigurationService(AlarmSystemConfigManager man) : this()
        {
            if(man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnUserConfig>(ConfigManager.Config.UserConfig, man.RemoteDevice);
            }

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

        public List<sconnUser> GetAll()
        {
            if (Online)
            {
                EntityManager.Download();
            }
            return ConfigManager.Config.UserConfig.Users.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnUser dev = this.GetById(Id);
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

        public sconnUser GetById(int Id)
        {
            try
            {
                if (Online)
                {
                    EntityManager.Download();
                }
                sconnUser dev = ConfigManager.Config.UserConfig.Users.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Add(sconnUser device)
        {
            try
            {
                ConfigManager.Config.UserConfig.Users.Add(device);
                return true;    //no adding -  filled with empty objects
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Update(sconnUser rcpt)
        {
            try
            {
                ConfigManager.Config.UserConfig.Users
                   .Where(z => z.Id == rcpt.Id)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.Login = rcpt.Login;
                       x.Password = rcpt.Password;
                       x.Enabled = rcpt.Enabled;
                       x.Group = rcpt.Group;
                       x.Permissions = rcpt.Permissions;
                       x.AllowedFrom = rcpt.AllowedFrom;
                       x.AllowedUntil = rcpt.AllowedUntil;
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

        public bool Remove(sconnUser device)
        {
            try
            {
                if (Online)
                {
                    // 'Remove' clears static record instead - replace with new empty record with the same Id
                    sconnUser stub = new sconnUser { Id = device.Id };
                    this.Update(stub);
                    return SaveChanges();
                }
                else
                {
                    this.ConfigManager.Config.UserConfig.Users.Remove(device);
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
