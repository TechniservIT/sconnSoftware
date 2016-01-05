using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;
using AlarmSystemManagmentService.AuthorizedDevices;
using NLog;

namespace AlarmSystemManagmentService
{
    public class AuthorizedDevicesConfigurationService : IAuthorizedDevicesConfigurationService  
    {
        public AlarmSystemConfigManager Manager { get; set; }
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AuthorizedDevicesConfigurationService()
        {

            Online = true; //online by default
        }

        public AuthorizedDevicesConfigurationService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadAuthorizedDevicesConfig();
            }
            else
            {
                return true;
            }
        }

        public List<sconnAuthorizedDevice> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.AuthorizedDevices.Devices.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnAuthorizedDevice dev = this.Manager.Config.AuthorizedDevices.Devices.Where(d => d.Id == Id).FirstOrDefault();
                if (dev != null)
                {
                    Manager.Config.AuthorizedDevices.Devices.Remove(dev);
                    return SaveChanges();
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

                sconnAuthorizedDevice dev =
                    this.Manager.Config.AuthorizedDevices.Devices.Where(d => d.Id == Id).FirstOrDefault();
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
                Manager.Config.AuthorizedDevices.Devices.Add(device);
                return SaveChanges();

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
                var odevice = Manager.Config.AuthorizedDevices.Devices.Where(z => z.Id == device.Id).FirstOrDefault();
                if (odevice != null)
                {
                    odevice = device;
                    Manager.Config.AuthorizedDevices.Devices.Add(device);
                    return SaveChanges();
                }
                else
                {
                    return false;
                }
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
                Manager.Config.AuthorizedDevices.Devices.Remove(device);
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
