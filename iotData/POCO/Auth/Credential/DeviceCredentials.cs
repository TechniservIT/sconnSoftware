using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceCredentials
    {
        [Key]
        [Required]
        [DataMember]
        public int CredentialId { get; set; }

        [DataMember]
        public virtual AppAuthLevel AuthLevel { get; set; }

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

        [DataMember]
        public virtual List<Device> Devices { get; set; }


    }
}