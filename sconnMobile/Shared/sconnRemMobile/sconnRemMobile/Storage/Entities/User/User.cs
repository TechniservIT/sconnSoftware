using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{

    [DataContract(IsReference = true)]
    public class User
    {
        [Key]
        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public string Surname { get; set; }


        [Required]
        [DataMember]
        public string EmailAddress { get; set; }


        public virtual  AppUserCredentials Credentials { get; set; }

        public virtual ICollection<IUserPermission> UserPermissions { get; set; }




    }
}