using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config;

namespace AlarmSystemManagmentService.IO.Relay
{
    public class RelayConfigService : IAlarmSystemConfigurationService<sconnRelay>
    {
        private AlarmSystemConfigManager Manager { get; set; }

        public RelayConfigService()
        {
        }

        public RelayConfigService(Device AlarmDevice) : this()
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public RelayConfigService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        public bool RemoveById(int Id)
        {
            sconnRelay dev = this.Manager.Config.RelayConfig.Relays.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.RelayConfig.Relays.Remove(dev);
                return Manager.UploadAuthorizedDevicesConfig();
            }
            return false;
        }

        public sconnRelay GetById(int Id)
        {
            sconnRelay dev = this.Manager.Config.RelayConfig.Relays.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }

        public List<sconnRelay> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.RelayConfig.Relays.ToList();
        }

        public bool Add(sconnRelay zone)
        {
            try
            {
                Manager.Config.RelayConfig.Relays.Add(zone);
                return Manager.UploadZoneConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnRelay zone)
        {
            try
            {
                var ozone = Manager.Config.RelayConfig.Relays.Where(z => z.Name.Equals(zone.Name)).FirstOrDefault();
                if (ozone != null)
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

        public bool Remove(sconnRelay zone)
        {
            try
            {
                Manager.Config.RelayConfig.Relays.Remove(zone);
                return Manager.UploadZoneConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
