using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using iotDash.Identity.Roles;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Models
{
    public class CreateRoleBindingModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

    }

    public class RoleListModel
    {

        public List<IotUserRole> Roles { get; set; }

        public RoleListModel(iotDomain domain)
        {
            try
            {
                ApplicationDbContext ucont = new ApplicationDbContext();
                var roleManager = new RoleManager<IotUserRole>(new RoleStore<IotUserRole>(ucont));
                Roles = roleManager.Roles.Where(r => r.DomainId == domain.Id).ToList();
            }
            catch (Exception)
            {
                return;
            }
        }
      
    }
    



    public class IotRoleModel
    {
        public List<Site> Sites { get; set; }

        public List<Device> Devices { get; set; }

        public iotDomain Domain { get; set; }

        public IotUserRole Role { get; set; }

        public string  Result { get; set; }

        
        public IotRoleModel()
        {
            this.Role = new IotUserRole();
        }


        public IotRoleModel(iotDomain domain): this()
        {
            try
            {
                Sites = domain.Sites.ToList();
                Devices = Sites.FirstOrDefault().Devices.ToList();
                Domain = domain;
                this.Role.DomainId = this.Domain.Id;
            }
            catch (Exception e)
            {
 
            }

        }

    }

    public class UsersInRoleModel
    {

        public string Id { get; set; }
        public List<string> EnrolledUsers { get; set; }
        public List<string> RemovedUsers { get; set; }
    }
}
