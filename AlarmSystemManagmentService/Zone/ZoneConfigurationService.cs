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

namespace AlarmSystemManagmentService
{
    public class ZoneConfigurationService : IZoneConfigurationService
    {
        private AlarmSystemConfigManager Manager { get; set; }

        public ZoneConfigurationService()
        {
        }

        public ZoneConfigurationService(Device AlarmDevice) : this()
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public ZoneConfigurationService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        public bool RemoveById(int Id)
        {
            sconnAlarmZone dev = this.Manager.Config.ZoneConfig.Zones.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.ZoneConfig.Zones.Remove(dev);
                return Manager.UploadAuthorizedDevicesConfig();
            }
            return false;
        }

        public sconnAlarmZone GetById(int Id)
        {
            sconnAlarmZone dev = this.Manager.Config.ZoneConfig.Zones.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }

        public List<sconnAlarmZone> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.ZoneConfig.Zones.ToList();
        }

        public bool Add(sconnAlarmZone zone)
        {
            try
            {
                Manager.Config.ZoneConfig.Zones.Add(zone);
                return Manager.UploadZoneConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnAlarmZone zone)
        {
            try
            {
                var ozone = Manager.Config.ZoneConfig.Zones.Where(z => z.Name.Equals(zone.Name)).FirstOrDefault();
                if(ozone != null)
                {
                    ozone = zone;
                    return Manager.UploadZoneConfig();
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

        public bool Remove(sconnAlarmZone zone)
        {
            try
            {
                Manager.Config.ZoneConfig.Zones.Remove(zone);
                return Manager.UploadZoneConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }


    }
}
