using System;
using System.Collections.Generic;
using System.Linq;
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

        private void LoadDeviceConfigs()
        {
            sconnGlobalConfig glob = new sconnGlobalConfig();
            AlarmGenericConfigManager<sconnGlobalConfig> gbman = new AlarmGenericConfigManager<sconnGlobalConfig>(glob, ConfigManager.RemoteDevice);
            gbman.Download();
            DeviceNo = glob.Devices;

            DeviceConfigs = new List<sconnDevice>();
            for (int i = 0; i < DeviceNo; i++)
            {
                sconnDeviceConfig dev = new sconnDeviceConfig();
                AlarmGenericConfigManager<sconnDeviceConfig> dman = new AlarmGenericConfigManager<sconnDeviceConfig>(dev, ConfigManager.RemoteDevice, i);
                dman.Download();
                DeviceConfigs.Add(dev.Device);
            }
        }

        public List<sconnDevice> GetAll()
        {
            LoadDeviceConfigs();
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
                ConfigManager.Config.DeviceConfig.Device.Inputs = rcpt.Inputs;
                ConfigManager.Config.DeviceConfig.Device.Outputs = rcpt.Outputs;
                ConfigManager.Config.DeviceConfig.Device.Relays = rcpt.Relays;
                ConfigManager.Config.DeviceConfig.Device.Name = rcpt.Name;
                ConfigManager.Config.DeviceConfig.Device.NetworkConfig = rcpt.NetworkConfig;
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
