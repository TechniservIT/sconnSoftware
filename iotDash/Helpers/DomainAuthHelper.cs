using iotDash.Identity;
using iotDash.Identity.Roles;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace iotDash.Helpers
{
    static public class DomainAuthHelper
    {
        static public bool IsUserContextDomainAuthorized(HttpContextBase httpContext)
        {
            string appdomain = DomainRoutingHelpers.GetDomainNameFromContext(httpContext);
            if (appdomain != null)
            {
                string username = httpContext.User.Identity.Name;
                ApplicationDbContext cont = new ApplicationDbContext();
                var user = (from u in cont.Users
                            where u.UserName == username
                            select u).First();

                var icont = (IIotContextBase)System.Web.HttpContext.Current.Session["iotcontext"];
                if (icont == null)
                {
                    icont = UserIotContextFactory.GetContextForUser(user);
                    System.Web.HttpContext.Current.Session["iotcontext"] = icont;
                }

                iotDomain domain = icont.Domains.First(dm => dm.DomainName.Equals(appdomain));
                if (domain != null)
                {
                    if (domain.DomainName.Equals(appdomain))
                    {
                        return true;    //user allowed to access domain
                    }
                }
            }

            return false;
        }



        static public bool UserHasAdminAccess(HttpContextBase cont)
        {
            ApplicationDbContext ucont = new ApplicationDbContext();
            iotDomain d = DomainSession.GetDomainForHttpContext(cont);
            var roleManager = new RoleManager<IotUserRole>(new RoleStore<IotUserRole>(ucont));
            IotUserRole DomainAdminRole = roleManager.Roles.Where(r => r.DomainId == d.Id && r.Type == IotUserRoleType.DomainAdmin).FirstOrDefault();
            return cont.User.IsInRole(DomainAdminRole.Name);
        }

        static public bool UserHasSiteAccess(HttpContextBase cont)
        {
            if (UserHasAdminAccess(cont))
            {
                return true;
            }

            return false;
        }

        static public bool UserHasDeviceAccess(HttpContextBase cont)
        {
            if (UserHasAdminAccess(cont))
            {
                return true;
            }
            return false;
        }

        static public bool UserHasDeviceAccess(Device dev, IPrincipal user)
        {
            if (user != null && dev != null)
            {
                ApplicationDbContext ucont = new ApplicationDbContext();
                 ApplicationUser nuser = ucont.Users.FirstOrDefault(u => u.UserName.Equals(user.Identity.Name));
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ucont));
               // var userName = user.Identity.GetUserName();
               // var nuser = userManager.FindByName<ApplicationUser>(userName);
                iotDomain d = dev.Site.Domain;
                if (nuser.DomainId == dev.Site.Domain.Id)   //user is accessed domain
                {
                    var roleManager = new RoleManager<IotUserRole>(new RoleStore<IotUserRole>(ucont));
                    IotUserRole DomainAdminRole = roleManager.Roles.Where(r => r.DomainId == d.Id && r.Type == IotUserRoleType.DomainAdmin).FirstOrDefault();
                    IotUserRole SiteRole = roleManager.Roles.Where(r => r.DomainId == d.Id && r.Type == IotUserRoleType.SiteManager).FirstOrDefault();
                    IotUserRole DeviceRoles = roleManager.Roles.Where(r => r.DomainId == d.Id && r.Type == IotUserRoleType.DeviceManager).FirstOrDefault();
                    return ((DomainAdminRole!= null) || (SiteRole!= null) || (DeviceRoles!= null));  //user has device access role
                }
            }
            return false;
        }

    }

}