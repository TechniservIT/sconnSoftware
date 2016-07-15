using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;
using AlarmSystemManagmentService;
using NLog;
using sconnConnector.Config.Abstract;

namespace AlarmSystemManagmentService
{
    public class AuthorizedDevicesConfigurationService : IAuthorizedDevicesConfigurationService  
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnAuthorizedDevicesConfig> EntityManager;
        private AlarmSystemConfigManager ConfigManager;

        public AuthorizedDevicesConfigurationService()
        {
            Online = true; //online by default
        }
        
        public AuthorizedDevicesConfigurationService(AlarmSystemConfigManager man) : this()
        {
            if(man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnAuthorizedDevicesConfig>(ConfigManager.Config.AuthorizedDevicesConfig, man.RemoteDevice);
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

        public List<sconnAuthorizedDevice> GetAll()
        {
            if (Online)
            {
                EntityManager.Download();
            }
            return ConfigManager.Config.AuthorizedDevicesConfig.Devices.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnAuthorizedDevice dev = this.GetById(Id);
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

        public sconnAuthorizedDevice GetById(int Id)
        {
            try
            {
                if (Online)
                {
                    EntityManager.Download();
                }
                sconnAuthorizedDevice dev = ConfigManager.Config.AuthorizedDevicesConfig.Devices.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Add(sconnAuthorizedDevice device)
        {
            try
            {
                ConfigManager.Config.AuthorizedDevicesConfig.Devices.Add(device);
                return true;    //no adding -  filled with empty objects
            }
            catch (Exception e )
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Update(sconnAuthorizedDevice device)
        {
            try
            {
                ConfigManager.Config.AuthorizedDevicesConfig.Devices
                   .Where(z => z.Id == device.Id)
                   .ToList()
                   .ForEach(x =>
                   {
                       x._Serial = device._Serial;
                       x._Enabled = device._Enabled;
                       x._AllowedFrom = device._AllowedFrom;
                       x._AllowedUntil = device._AllowedUntil;
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

        public bool Remove(sconnAuthorizedDevice device)
        {
            try
            {
                if (Online)
                {
                    // 'Remove' clears static record instead - replace with new empty record with the same Id
                    sconnAuthorizedDevice stub = new sconnAuthorizedDevice();
                    stub.Id = device.Id;
                    this.Update(stub);
                    return SaveChanges();
                }
                else
                {
                    this.ConfigManager.Config.AuthorizedDevicesConfig.Devices.Remove(device);
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
