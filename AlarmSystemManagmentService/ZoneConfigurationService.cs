using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService
{
    public class ZoneConfigurationService
    {
        private IIotContextBase context;
        private AlarmSystemConfigManager Manager { get; set; }

        public ZoneConfigurationService(IIotContextBase cont)
        {
            this.context = cont;
        }

        public ZoneConfigurationService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public ZoneConfigurationService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {
            Manager = man;
        }

        public sconnAlarmZoneConfig GetAlarmZoneConfig()
        {
            return Manager.Config.ZoneConfig;
        }

        public bool AddZone(sconnAlarmZone zone)
        {
            Manager.Config.ZoneConfig.Zones.Add(zone);
            return Manager.UploadZoneConfig();
        }


    }
}
