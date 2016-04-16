using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService
{
    public class GsmConfigurationService : IGsmConfigurationService
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnGsmConfig> EntityManager;
        private AlarmSystemConfigManager ConfigManager;

        public GsmConfigurationService()
        {
            Online = true; //online by default
        }

        public GsmConfigurationService(AlarmSystemConfigManager man) : this()
        {
            if(man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnGsmConfig>(ConfigManager.Config.GsmConfig, man.RemoteDevice);
            }

        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return EntityManager.Upload();
            }
            else
            {
                return true;
            }
        }

        public List<sconnGsmRcpt> GetAll()
        {
            if (Online)
            {
                EntityManager.Download();
            }
            return ConfigManager.Config.GsmConfig.Rcpts.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnGsmRcpt dev = this.GetById(Id);
                if (dev != null)
                {
                    return this.Remove(dev);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public sconnGsmRcpt GetById(int Id)
        {
            try
            {
                if (Online)
                {
                    EntityManager.Download();
                }
                sconnGsmRcpt dev = ConfigManager.Config.GsmConfig.Rcpts.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Add(sconnGsmRcpt device)
        {
            try
            {
                return true;    //no adding -  filled with empty objects
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Update(sconnGsmRcpt rcpt)
        {
            try
            {
                ConfigManager.Config.GsmConfig.Rcpts
                   .Where(z => z.Id == rcpt.Id)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.MessageLevel = rcpt.MessageLevel;
                       x.NumberE164 = rcpt.NumberE164;
                       x.CountryCode = rcpt.CountryCode;
                   }
                   );
                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Remove(sconnGsmRcpt device)
        {
            try
            {

                if (Online)
                {

                    // 'Remove' clears static record instead - replace with new empty record with the same Id
                    sconnGsmRcpt stub = new sconnGsmRcpt { Id = device.Id };
                    this.Update(stub);
                    return SaveChanges();
                }
                else
                {
                    this.ConfigManager.Config.GsmConfig.Rcpts.Remove(device);
                    return true;
                }

            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

    }
}
