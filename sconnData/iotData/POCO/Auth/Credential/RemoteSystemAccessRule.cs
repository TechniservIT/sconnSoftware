using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;

namespace iotData.POCO.Auth.Credential
{
    public class RemoteSystemAccessRule
    {
        [Key]
        [Required]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public virtual AppAuthLevel AuthLevel { get; set; }
        

        [Required]
        [DataMember]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DataMember]
        [DisplayName("Password")]
        public string Password { get; set; }
        

        [DataMember]
        [DisplayName("Permission Expiration")]
        public DateTime PermissionExpireDate { get; set; }

        [DataMember]
        [DisplayName("Password Expiration")]
        public DateTime PasswordExpireDate { get; set; }
        

        public RemoteSystemAccessRule()
        {
            PermissionExpireDate = DateTime.Now.AddYears(1000).Date;
            PasswordExpireDate = DateTime.Now.AddYears(1000).Date;
        }

    }
}
