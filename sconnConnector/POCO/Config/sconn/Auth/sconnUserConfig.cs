using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnUserConfig
    {
        public List<sconnUser> Users { get; set; }

        public sconnUserConfig()
        {
                Users = new List<sconnUser>();
        }

        public sconnUserConfig(ipcSiteConfig cfg) :this()
        {
            if (cfg.UserConfig.Length >= ipcDefines.AUTH_MAX_USERS*ipcDefines.AUTH_CRED_SIZE)
            {
                for (int i = 0; i < ipcDefines.AUTH_MAX_USERS; i++)
                {
                    byte[] authrec = new byte[ipcDefines.AUTH_CRED_SIZE];
                    for (int j = 0; j < ipcDefines.AUTH_CRED_SIZE; j++)
                    {
                        authrec[j] = cfg.UserConfig[i*ipcDefines.AUTH_CRED_SIZE + j];
                    }
                   sconnUser user = new sconnUser(authrec);
                    user.Id = i;
                    Users.Add(user);
                }
            }
        }

    }
}
