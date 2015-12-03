using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDash.Identity.Roles;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Models
{
    public class UserListModel
    {

        public List<ApplicationUser> Users { get; set; }

        public UserListModel(iotDomain domain)
        {
            try
            {
                ApplicationDbContext ucont = new ApplicationDbContext();
                var userMan = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                Users = userMan.Users.Where(u => u.DomainId == domain.Id).ToList();
            }
            catch (Exception)
            {
                return;
            }  
        }



    }

    public class UserEditModel
    {
        public ApplicationUser User { get; set; }

        public UserEditModel()
        {
                
        }
    }


    public class UserCreateModel
    {
        public ApplicationUser User { get; set; }

        public string Password { get; set; }

        public string Result { get; set; }


        public List<IotUserRole> Roles { get; set; } 

        public UserCreateModel()
        {
                
        }

        public UserCreateModel(iotDomain domain)
        {
            User = new ApplicationUser();
            ApplicationDbContext ucont = new ApplicationDbContext();
            var roleManager = new RoleManager<IotUserRole>(new RoleStore<IotUserRole>(ucont));
            Roles = roleManager.Roles.Where(r => r.DomainId == domain.Id).ToList();
            User.DomainId = domain.Id;
        }
    }

}
