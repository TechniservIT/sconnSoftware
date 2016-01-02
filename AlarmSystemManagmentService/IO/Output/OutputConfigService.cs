using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config;

namespace AlarmSystemManagmentService.IO.Output
{
    public class OutputConfigService : IAlarmSystemConfigurationService<sconnOutput>
    {
        private AlarmSystemConfigManager Manager { get; set; }
        public bool Online { get; set; }

        public OutputConfigService()
        {
            Online = true; //online by default
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadOutputsConfig();
            }
            else
            {
                return true;
            }
        }

        public OutputConfigService(Device AlarmDevice) : this()
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public OutputConfigService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        public bool RemoveById(int Id)
        {
            sconnOutput dev = this.Manager.Config.OutputConfig.Outputs.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.OutputConfig.Outputs.Remove(dev);
                return SaveChanges();
            }
            return false;
        }

        public sconnOutput GetById(int Id)
        {
            sconnOutput dev = this.Manager.Config.OutputConfig.Outputs.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }

        public List<sconnOutput> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.OutputConfig.Outputs.ToList();
        }

        public bool Add(sconnOutput zone)
        {
            try
            {
                Manager.Config.OutputConfig.Outputs.Add(zone);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnOutput zone)
        {
            try
            {
                var ozone = Manager.Config.OutputConfig.Outputs.Where(z => z.Name.Equals(zone.Name)).FirstOrDefault();
                if (ozone != null)
                {
                    ozone = zone;
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

        public bool Remove(sconnOutput zone)
        {
            try
            {
                Manager.Config.OutputConfig.Outputs.Remove(zone);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
