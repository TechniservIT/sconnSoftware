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
        public bool Online { get; set; }

        public RelayConfigService()
        {
            Online = true; //online by default
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadRelaysConfig();
            }
            else
            {
                return true;
            }
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
                return SaveChanges();
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
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnRelay relay)
        {
            try
            {
                var ozone = Manager.Config.RelayConfig.Relays.Where(z => z.Id == relay.Id).FirstOrDefault();
                if (ozone != null)
                {
                    ozone = relay;
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

        public bool Remove(sconnRelay zone)
        {
            try
            {
                Manager.Config.RelayConfig.Relays.Remove(zone);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
