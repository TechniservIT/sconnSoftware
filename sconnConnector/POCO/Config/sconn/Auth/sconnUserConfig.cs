using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnUserConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnUser> Users { get; set; }

        public sconnUserConfig()
        {
                Users = new List<sconnUser>();
        }

        public sconnUserConfig(ipcSiteConfig cfg) :this()
        {
            this.Deserialize(cfg.UserConfig);
        }
        
        public byte[] Serialize()
        {
            byte[] Serialized = new byte[ipcDefines.AUTH_RECORDS_SIZE];
            for (int i = 0; i < Users.Count; i++)
            {
                byte[] partial = Users[i].Serialize();
                partial.CopyTo(Serialized, i * ipcDefines.AUTH_RECORD_SIZE);
            }
            return Serialized;
        }

        public void Deserialize(byte[] buffer)
        {
            for (int i = 0; i < ipcDefines.AUTH_MAX_USERS; i++)
            {
                byte[] relayCfg = new byte[ipcDefines.AUTH_RECORD_SIZE];
                for (int j = 0; j < ipcDefines.AUTH_RECORD_SIZE; j++)
                {
                    relayCfg[j] = buffer[i * ipcDefines.AUTH_RECORD_SIZE+j];
                }
                sconnUser relay = new sconnUser(relayCfg);
                relay.Id = i;
                Users.Add(relay);
            }
        }

        public void Fake()
        {
            sconnUser zone = new sconnUser();
            zone.Fake();
            Users.Add(zone);
        }


    }
}
