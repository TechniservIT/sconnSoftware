using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDash.Identity.Roles;
using iotDash.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace iotDash.Controllers.domain.managment.security
{
    public class ApplicationRoleManager : RoleManager<IotUserRole>
    {
        public ApplicationRoleManager(IRoleStore<IotUserRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IotUserRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }

}
