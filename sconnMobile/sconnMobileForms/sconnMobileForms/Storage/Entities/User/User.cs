using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
   

    public class User
    {
        [Key]
        [Required]
         
        public string Name { get; set; }

        [Required]
         
        public string Surname { get; set; }


        [Required]
         
        public string EmailAddress { get; set; }


        public virtual  AppUserCredentials Credentials { get; set; }

        public virtual ICollection<IUserPermission> UserPermissions { get; set; }




    }
}