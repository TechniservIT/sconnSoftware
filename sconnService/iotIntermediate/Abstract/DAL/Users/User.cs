using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iodash.Models.Auth;
using iodash.Models.Application.Users;
using iodash.Models.Auth.Credential;

namespace iodash.Models.Application
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public virtual  AppUserCredentials Credentials { get; set; }

        public virtual ICollection<IUserPermission> UserPermissions { get; set; }




    }
}