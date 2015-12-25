using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;

namespace AlarmSystemManagmentService
{
    public class DeviceConfigService
    {
        private IIotContextBase context;

        public AlarmSystemConfigManager Manager { get; set; }

        public DeviceConfigService(IIotContextBase cont)
        {
            this.context = cont;
        }

        public DeviceConfigService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public DeviceConfigService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {
            Manager = man;
        }

    }
}
