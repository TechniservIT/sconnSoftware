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
    public class GsmConfigurationService
    {
        private IIotContextBase context;

        public AlarmSystemConfigManager Manager { get; set; }

        public GsmConfigurationService(IIotContextBase cont)
        {
            this.context = cont;
        }

        public GsmConfigurationService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public GsmConfigurationService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {
            Manager = man;
        }

        public bool AddRcpt(sconnGsmRcpt rcpt)
        {
            Manager.Config.GsmConfig.Rcpts.Add(rcpt);
            return Manager.UploadSiteConfig();
        }

        public sconnGsmConfig GetGsmConfig()
        {
            return Manager.Config.GsmConfig;
        }

    }
}
