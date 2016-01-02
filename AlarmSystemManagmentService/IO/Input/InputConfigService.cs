using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService.IO.Input
{
    public class InputConfigService : IAlarmSystemConfigurationService<sconnInput>
    {
        private AlarmSystemConfigManager Manager { get; set; }
        public bool Online { get; set; }

        public InputConfigService()
        {
            Online = true; //online by default
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadInputsConfig();
            }
            else
            {
                return true;
            }
        }


        public InputConfigService(Device AlarmDevice) : this()
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public InputConfigService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }

        public bool RemoveById(int Id)
        {
            sconnInput dev = this.Manager.Config.InputConfig.Inputs.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.InputConfig.Inputs.Remove(dev);
                return SaveChanges();
            }
            return false;
        }

        public sconnInput GetById(int Id)
        {
            sconnInput dev = this.Manager.Config.InputConfig.Inputs.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }

        public List<sconnInput> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.InputConfig.Inputs.ToList();
        }

        public bool Add(sconnInput zone)
        {
            try
            {
                Manager.Config.InputConfig.Inputs.Add(zone);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnInput zone)
        {
            try
            {
                var ozone = Manager.Config.InputConfig.Inputs.Where(z => z.Name.Equals(zone.Name)).FirstOrDefault();
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

        public bool Remove(sconnInput zone)
        {
            try
            {
                Manager.Config.InputConfig.Inputs.Remove(zone);
                return SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

        }
    }

}
