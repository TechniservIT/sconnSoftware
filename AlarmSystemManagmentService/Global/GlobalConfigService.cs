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
    public class GlobalConfigService : IGlobalConfigService
    {
        private IIotContextBase context;
        public bool Online { get; set; }
        public AlarmSystemConfigManager Manager { get; set; }

        public GlobalConfigService()
        {
            Online = true; //online by default
        }

        public GlobalConfigService(IIotContextBase cont)
        {
            this.context = cont;
        }

        private bool SaveChanges()
        {
            if (Online)
            {
                return Manager.UploadGlobalConfig();
            }
            else
            {
                return true;
            }
        }

        public GlobalConfigService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public GlobalConfigService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {
            Manager = man;
            Manager.LoadSiteConfig();
        }

    }
}
