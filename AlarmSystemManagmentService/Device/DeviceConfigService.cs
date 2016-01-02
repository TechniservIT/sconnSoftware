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
    public class DeviceConfigService : IAlarmSystemConfigurationService<sconnDevice>
    {
        public AlarmSystemConfigManager Manager { get; set; }
        public bool Online { get; set; }

        public DeviceConfigService()
        {
            Online = true; //online by default
        }
        
        public DeviceConfigService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadDeviceConfig();
            }
            else
            {
                return true;
            }
        }

        private bool SaveChanges(sconnDevice dev)
        {
            if (Online)
            {
                return Manager.UploadDeviceConfig(dev);
            }
            else
            {
                return true;
            }
        }

        public List<sconnDevice> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.DeviceConfig.Devices.ToList();
        }

        public bool RemoveById(int Id)
        {
            sconnDevice dev = this.Manager.Config.DeviceConfig.Devices.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.DeviceConfig.Devices.Remove(dev);
                return SaveChanges();
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
                return SaveChanges(device);
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
                var odevice = Manager.Config.DeviceConfig.Devices.Where(z => z.Id == device.Id).FirstOrDefault();
                if (odevice != null)
                {
                    odevice = device;
                    return SaveChanges(odevice);
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
                return SaveChanges(device);
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
