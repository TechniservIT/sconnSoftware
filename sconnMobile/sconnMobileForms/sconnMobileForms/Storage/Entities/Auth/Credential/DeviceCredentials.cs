using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
     
    public class DeviceCredentials
    {
        [Key]
        [Required]
         
        public int Id { get; set; }

         
        public virtual AppAuthLevel AuthLevel { get; set; }

        [Required]
         
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
         
        [DisplayName("Password")]
        public string Password { get; set; }

         
        public byte[] HashData { get; set; }

         
        [DisplayName("Permission Expiration")]
        public DateTime PermissionExpireDate { get; set; }

         
        [DisplayName("Password Expiration")]
        public DateTime PasswordExpireDate { get; set; }

         
        public virtual List<Device> Devices { get; set; }

        public DeviceCredentials()
        {
            PermissionExpireDate = DateTime.Now.AddYears(1000).Date;
            PasswordExpireDate = DateTime.Now.AddYears(1000).Date;
        }


        public void Fake()
        {
            Username = Guid.NewGuid().ToString();
            Password = Guid.NewGuid().ToString();
        }


    }
}