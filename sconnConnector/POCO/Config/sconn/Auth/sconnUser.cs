using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnUser : IAlarmSystemConfigurationEntity
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int Permissions { get; set; }

        public int Group { get; set; }

        public bool Enabled { get; set; }

        public DateTime AllowedFrom { get; set; }

        public DateTime AllowedUntil { get; set; }

        public byte[] Serialize()
        {
            //TODO
            return new byte[0];
        }

        public sconnUser()
        {
           
        }

        public sconnUser(byte[] serialized)
        {
            try
            {
                Permissions = serialized[ipcDefines.AUTH_RECORD_PERM_POS];
                Enabled = serialized[ipcDefines.AUTH_RECORD_ENABLED_POS] > 0 ? true : false;
                Group = serialized[ipcDefines.AUTH_RECORD_GROUP_POS];

                byte passLen = serialized[ipcDefines.AUTH_RECORD_PASS_LEN_POS];
                byte[] passwdBf = new byte[ipcDefines.AUTH_RECORD_PASSWD_LEN];
                for (int i = 0; i < passLen * 2; i++)
                {
                    passwdBf[i] = serialized[ipcDefines.AUTH_RECORD_PASSWD_POS + i];
                }
                Password = Encoding.UTF8.GetString(passwdBf);

                byte logLen = serialized[ipcDefines.AUTH_RECORD_LOGIN_LEN];
                byte[] logBf = new byte[ipcDefines.AUTH_PASS_SIZE];
                for (int i = 0; i < logLen * 2; i++)
                {
                    logBf[i] = serialized[ipcDefines.AUTH_RECORD_LOGIN_POS + i];
                }
                Login = Encoding.UTF8.GetString(logBf);


            }
            catch (Exception)
            {
            }

        }
    }
}
