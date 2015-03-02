using iodash.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Auth.Credential
{
    public class DeviceCredentials
    {
        [Key]
        [Required]
        public int CredentialId { get; set; }

        public virtual AppAuthLevel AuthLevel { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public byte[] HashData { get; set; }

        public DateTime PermissionExpireDate { get; set; }

        public DateTime PasswordExpireDate { get; set; }

        public virtual ICollection<Device> Devices { get; set; }


    }
}