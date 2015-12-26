using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract.Auth;

namespace AlarmSystemManagmentService
{
    public class UsersConfigurationService
    {
        private IIotContextBase context;

        public AlarmSystemConfigManager Manager { get; set; }

        public UsersConfigurationService(IIotContextBase cont)
        {
            this.context = cont;
        }

        public UsersConfigurationService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public UsersConfigurationService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {
            Manager = man;
        }

        public sconnUserConfig GetUserConfig()
        {
            return Manager.Config.UserConfig;
        }

        public bool AddUser(sconnUser user)
        {
            Manager.Config.UserConfig.Users.Add(user);
            return Manager.UploadSiteConfig();
        }

    }
}
