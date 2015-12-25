using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;

namespace AlarmSystemManagmentService
{
    public class AuthorizedDevicesConfigurationService
    {
        private IIotContextBase context;

        public AlarmSystemConfigManager Manager { get; set; }
        
        public AuthorizedDevicesConfigurationService(IIotContextBase cont)
        {
            this.context = cont;
        }
        
        public AuthorizedDevicesConfigurationService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public AlarmSystemAuthorizedDevicesConfig GetAuthorizedDevices()
        {
            AlarmSystemAuthorizedDevicesConfig cfg = new AlarmSystemAuthorizedDevicesConfig();
            return cfg;
        }

    }

}
