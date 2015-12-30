using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService
{
    public class GsmConfigurationService : IGsmConfigurationService
    {
        public AlarmSystemConfigManager Manager { get; set; }

        public GsmConfigurationService()
        {
        }

        public GsmConfigurationService(Device AlarmDevice) : this()
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public GsmConfigurationService(AlarmSystemConfigManager man) : this()
        {
            Manager = man;
        }


        public bool RemoveById(int Id)
        {
            sconnGsmRcpt dev = this.Manager.Config.GsmConfig.Rcpts.Where(d => d.Id == Id).FirstOrDefault();
            if (dev != null)
            {
                Manager.Config.GsmConfig.Rcpts.Remove(dev);
                return Manager.UploadAuthorizedDevicesConfig();
            }
            return false;
        }

        public sconnGsmRcpt GetById(int Id)
        {
            sconnGsmRcpt dev = this.Manager.Config.GsmConfig.Rcpts.Where(d => d.Id == Id).FirstOrDefault();
            return dev;
        }


        public List<sconnGsmRcpt> GetAll()
        {
            Manager.LoadSiteConfig();
            return Manager.Config.GsmConfig.Rcpts.ToList();
        }

        public bool Add(sconnGsmRcpt rcpt)
        {
            try
            {
                Manager.Config.GsmConfig.Rcpts.Add(rcpt);
                return Manager.UploadGsmConfig();
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(sconnGsmRcpt rcpt)
        {
            try
            {
                var oldrcpt = Manager.Config.GsmConfig.Rcpts.Where(z => z.NumberE164.Equals(rcpt.NumberE164)).FirstOrDefault();
                if (oldrcpt != null)
                {
                    oldrcpt = rcpt;
                    return Manager.UploadGsmConfig();
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

        public bool Remove(sconnGsmRcpt rcpt)
        {
            try
            {
                Manager.Config.GsmConfig.Rcpts.Remove(rcpt);
                return Manager.UploadGsmConfig();
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
