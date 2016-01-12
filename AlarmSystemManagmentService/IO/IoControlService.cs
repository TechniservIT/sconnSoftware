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
    public class IoControlService
    {
        private IIotContextBase context;

        public AlarmSystemConfigManager Manager { get; set; }

        public IoControlService(IIotContextBase cont)
        {
            this.context = cont;
        }

        public IoControlService(IIotContextBase cont, iotDbConnector.DAL.Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public IoControlService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {
            Manager = man;
        }

    }
}
