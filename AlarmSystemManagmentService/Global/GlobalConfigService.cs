using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService
{
    public class GlobalConfigService : IAlarmSystemSingleEntityConfigurationService<sconnGlobalConfig>
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnGlobalConfig> EntityManager;
        public AlarmSystemConfigManager ConfigManager;

        public GlobalConfigService()
        {
            Online = true; //online by default
        }

        public GlobalConfigService(AlarmSystemConfigManager man) : this()
        {
            if(man != null)
            {
                ConfigManager = man;
                EntityManager = new AlarmGenericConfigManager<sconnGlobalConfig>(ConfigManager.Config.GlobalConfig, man.RemoteDevice);
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

        public sconnGlobalConfig Get()
        {
            if (Online)
            {
                EntityManager.Download();
            }
            return ConfigManager.Config.GlobalConfig;
        }
        

        public bool Update(sconnGlobalConfig rcpt)
        {
            try
            {
                ConfigManager.Config.GlobalConfig.Armed = rcpt.Armed;
                ConfigManager.Config.GlobalConfig.Devices = rcpt.Devices;
                ConfigManager.Config.GlobalConfig.Lat = rcpt.Lat;
                ConfigManager.Config.GlobalConfig.Lng = rcpt.Lng;
                ConfigManager.Config.GlobalConfig.Violation = rcpt.Violation;
                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Remove(sconnGlobalConfig device)
        {
            try
            {
                // 'Remove' clears static record instead - replace with new empty record with the same Id
                sconnGlobalConfig stub = new sconnGlobalConfig { Id = device.Id };
                this.Update(stub);
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
