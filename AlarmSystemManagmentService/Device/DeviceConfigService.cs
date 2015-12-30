using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config;

namespace AlarmSystemManagmentService
{
    public class DeviceConfigService : IDeviceConfigService
    {
        public AlarmSystemConfigManager Manager { get; set; }
        
        public DeviceConfigService()
        {

        }
        
        public DeviceConfigService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        public List<sconnDevice> GetAll()
        {
            return Manager.Config.DeviceConfig.Devices.ToList();
        }

        public bool RemoveById(int Id)
        {
            sconnDevice dev = this.Manager.Config.DeviceConfig.Devices.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.DeviceConfig.Devices.Remove(dev);
                return Manager.UploadAuthorizedDevicesConfig();
            }
            return false;
        }

        public sconnDevice GetById(int Id)
        {
            sconnDevice dev = this.Manager.Config.DeviceConfig.Devices.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }


        public bool Add(sconnDevice device)
        {
            try
            {
                Manager.Config.DeviceConfig.Devices.Add(device);
                return Manager.UploadDeviceConfig(device);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnDevice device)
        {
            try
            {
                var odevice = Manager.Config.DeviceConfig.Devices.Where(z => z.DeviceId.Equals(device.DeviceId)).FirstOrDefault();
                if (odevice != null)
                {
                    odevice = device;
                    return Manager.UploadDeviceConfig(odevice);
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

        public bool Remove(sconnDevice device)
        {
            try
            {
                Manager.Config.DeviceConfig.Devices.Remove(device);
                return Manager.UploadDeviceConfig(device);
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
