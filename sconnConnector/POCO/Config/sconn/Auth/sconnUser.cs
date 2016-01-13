using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config.Abstract.Auth
{

    public enum sconnUserGroup
    {
        Admin = 1,
        ZoneAdmin,
        Service,
        TA
    }


    public class sconnUser : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {

        public int Id { get; set; }

        [Required]
        [DisplayName("Login")]
        public string Login { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Permissions")]
        public int Permissions { get; set; }

        [Required]
        [DisplayName("Group")]
        public sconnUserGroup Group { get; set; }

        [Required]
        [DisplayName("Enabled")]
        public bool Enabled { get; set; }

        [Required]
        [DisplayName("Allowed From")]
        public DateTime AllowedFrom { get; set; }

        [Required]
        [DisplayName("Allowed Until")]
        public DateTime AllowedUntil { get; set; }

        public int Value { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnUser()
        {
           
        }

        public sconnUser(byte[] serialized)
        {
            this.Deserialize(serialized);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.AUTH_RECORD_SIZE];
                buffer[ipcDefines.AUTH_RECORD_PERM_POS] = (byte)Permissions;
                buffer[ipcDefines.AUTH_RECORD_ENABLED_POS] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.AUTH_RECORD_GROUP_POS] = (byte)Group;
                buffer[ipcDefines.AUTH_RECORD_PASS_LEN_POS] = (byte)this.Password.Length;
                char[] passB = this.Password.ToCharArray();
                for (int i = 0; i < passB.Length; i++)
                {
                    buffer[ipcDefines.AUTH_RECORD_PASSWD_POS + i] = (byte)passB[i];
                }
                char[] logB = this.Login.ToCharArray();
                for (int i = 0; i < logB.Length; i++)
                {
                    buffer[ipcDefines.AUTH_RECORD_LOGIN_POS + i] = (byte)logB[i];
                }

                return buffer;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                if (buffer.Length >= ipcDefines.AUTH_CRED_SIZE)
                {
                    Permissions = buffer[ipcDefines.AUTH_RECORD_PERM_POS];
                    Enabled = buffer[ipcDefines.AUTH_RECORD_ENABLED_POS] > 0 ? true : false;
                    Group = (sconnUserGroup)buffer[ipcDefines.AUTH_RECORD_GROUP_POS];

                    byte passLen = buffer[ipcDefines.AUTH_RECORD_PASS_LEN_POS];
                    byte[] passwdBf = new byte[ipcDefines.AUTH_RECORD_PASSWD_LEN];
                    for (int i = 0; i < passLen * 2; i++)
                    {
                        passwdBf[i] = buffer[ipcDefines.AUTH_RECORD_PASSWD_POS + i];
                    }
                    Password = Encoding.UTF8.GetString(passwdBf);

                    byte logLen = buffer[ipcDefines.AUTH_RECORD_LOGIN_LEN];
                    byte[] logBf = new byte[ipcDefines.AUTH_PASS_SIZE];
                    for (int i = 0; i < logLen * 2; i++)
                    {
                        logBf[i] = buffer[ipcDefines.AUTH_RECORD_LOGIN_POS + i];
                    }
                    Login = Encoding.UTF8.GetString(logBf);
                } 
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

        public void Fake()
        {
            try
            {
                this.Id = 0;
                this.Enabled = true;
                this.Login = Guid.NewGuid().ToString();
                this.Password = Guid.NewGuid().ToString();
                this.AllowedFrom = DateTime.MinValue;
                this.AllowedUntil = DateTime.MaxValue;
                this.Group = sconnUserGroup.Admin;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }


        }

    }
}
