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

namespace AlarmSystemManagmentService
{
    public class AuthorizedDevicesConfigurationService : IAuthorizedDevicesConfigurationService  
    {
        public AlarmSystemConfigManager Manager { get; set; }
        
        public AuthorizedDevicesConfigurationService()
        {
        }

        public AuthorizedDevicesConfigurationService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }
        
        public List<sconnAuthorizedDevice> GetAll()
        {
            return Manager.Config.AuthorizedDevices.Devices.ToList();
        }

        public bool Add(sconnAuthorizedDevice device)
        {
            try
            {
                Manager.Config.AuthorizedDevices.Devices.Add(device);
                return Manager.UploadAuthorizedDevicesConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnAuthorizedDevice device)
        {
            try
            {
                var odevice = Manager.Config.AuthorizedDevices.Devices.Where(z => z._Serial.Equals(device._Serial)).FirstOrDefault();
                if (odevice != null)
                {
                    odevice = device;
                    return Manager.UploadAuthorizedDevicesConfig();
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

        public bool Remove(sconnAuthorizedDevice device)
        {
            try
            {
                Manager.Config.AuthorizedDevices.Devices.Remove(device);
                return Manager.UploadAuthorizedDevicesConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }

    }


}
