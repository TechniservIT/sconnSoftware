using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using iotDash.Identity.Roles;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Models
{
    public class UserListModel : IAsyncStatusModel
    {

        public List<ApplicationUser> Users { get; set; }
        public string Result { get; set; }

        public UserListModel(iotDomain domain)
        {
            try
            {
                ApplicationDbContext ucont = new ApplicationDbContext();
                var userMan = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                Users = userMan.Users.Where(u => u.DomainId == domain.Id).ToList();
            }
            catch (Exception e)
            {
                return;
            }  
        }



    }

    public class UserEditModel : IAsyncStatusModel
    {
        public ApplicationUser User { get; set; }
        public string Result { get; set; }

        public UserEditModel()
        {
                
        }
    }


    public class UserCreateModel : IAsyncStatusModel
    {
        public ApplicationUser User { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Result { get; set; }

        //public string[] SelectedIds { get; set; }
        //public IEnumerable<SelectListItem> Roles { get; set; } 
        //public MultiSelectList Roles { get; set; }
        public List<IotUserRole> Roles { get; set; } 
        

        public UserCreateModel()
        {
                
        }

        public UserCreateModel(iotDomain domain)
        {
            User = new ApplicationUser();
            ApplicationDbContext ucont = new ApplicationDbContext();
            var roleManager = new RoleManager<IotUserRole>(new RoleStore<IotUserRole>(ucont));
            var roleList = roleManager.Roles.Where(r => r.DomainId == domain.Id).ToList();
            Roles = roleList;
            foreach (var role in this.Roles)
            {
                role.Active = false;    //use activation as user role enable and default all to false
            }
            User.DomainId = domain.Id;
        }
    }

}
