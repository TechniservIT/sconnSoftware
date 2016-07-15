using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Config;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService.Device
{
    public class AlarmDevicesConfigService : IAlarmSystemConfigurationService<sconnDevice>
    {
        public bool Online { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private AlarmGenericConfigManager<sconnDeviceConfig> EntityManager;

        private int DeviceNo;
        private List<sconnDevice> DeviceConfigs; 

        public AlarmSystemConfigManager ConfigManager;

        public AlarmDevicesConfigService()
        {
            Online = true; //online by default
        }
        
        public AlarmDevicesConfigService(AlarmSystemConfigManager man) : this()
        {
            ConfigManager = man;
            EntityManager = new AlarmGenericConfigManager<sconnDeviceConfig>(ConfigManager.Config.DeviceConfig, man.RemoteDevice);
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

        public List<sconnDevice> GetAll()
        {
            this.EntityManager.Download();
            return DeviceConfigs;
        }

        
        public sconnDevice GetById(int Id)
        {
            if (Id <= DeviceConfigs.Count)
            {
                return DeviceConfigs[Id];
            }
            else
            {
                return null;
            }
        }

        public bool RemoveById(int Id)
        {
            return false;
        }

        public bool Add(sconnDevice entity)
        {
            return false;   //adding virtual devices is not supported 
        }

        public bool Update(sconnDevice rcpt)
        {
            try
            {
                sconnDevice dev = null;
                foreach (var sdev in ConfigManager.Config.DeviceConfig.Devices)
                {
                    if (sdev.Id == rcpt.Id)
                    {
                        dev = sdev;
                    }
                }
                if (dev != null)
                {
                    dev.Inputs = rcpt.Inputs;
                    dev.Outputs = rcpt.Outputs;
                    dev.Relays = rcpt.Relays;
                    dev.Name = rcpt.Name;
                    dev.NetworkConfig = rcpt.NetworkConfig;
                }

                return SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public bool Remove(sconnDevice device)
        {
            try
            {
                // 'Remove' clears static record instead - replace with new empty record with the same Id
                sconnDevice stub = new sconnDevice { Id = device.Id };
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
