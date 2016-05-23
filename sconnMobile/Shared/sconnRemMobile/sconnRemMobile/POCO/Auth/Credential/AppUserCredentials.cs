using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class AppUserCredentials
    {
        [Key]
        [Required]
        [DataMember]
        public int CredentialId { get; set; }

        [Required]
        [DataMember]
        public virtual User CredentialUser { get; set; }

        [Required]
        [DataMember]

        public string Username { get; set; }

        [Required]
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public byte[] HashData { get; set; }

        [DataMember]
        public DateTime PermissionExpireDate { get; set; }

        [DataMember]
        public DateTime PasswordExpireDate { get; set; }


    }
}