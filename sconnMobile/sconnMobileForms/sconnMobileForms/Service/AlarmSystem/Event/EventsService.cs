using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService.Event
{
    public class EventsService
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnEventConfig> EntityManager;
        public AlarmSystemConfigManager ConfigManager;

        public EventsService()
        {
            Online = true; //online by default
        }

        public EventsService(AlarmSystemConfigManager man) : this()
        {
            ConfigManager = man;
            EntityManager = new AlarmGenericConfigManager<sconnEventConfig>(ConfigManager.Config.EventConfig, man.RemoteDevice);
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
        

        public List<sconnEvent> GetAll()
        {
            EntityManager.Download();
            return ConfigManager.Config.EventConfig.Events.ToList();
        }

        public bool RemoveById(int Id)
        {
            try
            {
                sconnEvent dev = this.GetById(Id);
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

        public sconnEvent GetById(int Id)
        {
            try
            {
                EntityManager.Download();
                sconnEvent dev = ConfigManager.Config.EventConfig.Events.FirstOrDefault(d => d.Id == Id);
                return dev;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }


        public bool Remove(sconnEvent device)
        {
            try
            {
                this.ConfigManager.Config.EventConfig.Events.Remove(device);
                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }


    }

}
