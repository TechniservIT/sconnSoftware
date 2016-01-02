using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract.Auth;

namespace AlarmSystemManagmentService
{
    public class UsersConfigurationService : IUsersConfigurationService
    {

        public AlarmSystemConfigManager Manager { get; set; }
        public bool Online { get; set; }

        public UsersConfigurationService()
        {
            Online = true; //online by default
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadUserConfig();
            }
            else
            {
                return true;
            }
        }

        public UsersConfigurationService(Device AlarmDevice) : this()
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public UsersConfigurationService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        public bool RemoveById(int Id)
        {
            sconnUser dev = this.Manager.Config.UserConfig.Users.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.UserConfig.Users.Remove(dev);
                return SaveChanges();
            }
            return false;
        }

        public sconnUser GetById(int Id)
        {
            sconnUser dev = this.Manager.Config.UserConfig.Users.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }

        public List<sconnUser> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.UserConfig.Users.ToList();
        }

        public bool Add(sconnUser user)
        {
            try
            {
                Manager.Config.UserConfig.Users.Add(user);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnUser user)
        {
            try
            {
                var oldUser = Manager.Config.UserConfig.Users.Where(z => z.Login.Equals(user.Login)).FirstOrDefault();
                if (oldUser != null)
                {
                    oldUser = user;
                    return SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Remove(sconnUser user)
        {
            try
            {
                Manager.Config.UserConfig.Users.Remove(user);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
}
