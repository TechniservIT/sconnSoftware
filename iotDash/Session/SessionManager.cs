using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

using iotDbConnector.DAL;
using iotServiceProvider;
using iotDeviceService;

namespace iotDash.Session
{
    static public class SessionManager
    {

        static public bool IsUserLoggedIn()
        {
            return true;
        }

        static public string AppDomainForCurrentUser()
        {
            string domain = "";
            return domain;
        }

        static public string AppDomainNameForContext(HttpContext cont)
        {
            return "";
        }
        static public string AppDomainNameForContext(HttpContextBase cont)
        {
            return "";
        }

        static public iotDomain AppDomainForUserContext(HttpContextBase cont)
        {
            try
            {
                IPrincipal seesionAuth = cont.User;
                string username = seesionAuth.Identity.Name;
                ApplicationUser user = GetUserWithName(username);
                if (user != null)
                {
                    DeviceRestfulService cl = new DeviceRestfulService();
                    string domainId = DomainSession.GetContextDomain(cont);
                    iotDomain domain = cl.GetDomainWithName(domainId);
                    return domain;
                }
                else
                {
                    return new iotDomain();
                }
            }
            catch (Exception e)
            {
                return new iotDomain();
            }
        }

        static ApplicationUser GetUserWithName(string uname)
        {
            ApplicationDbContext cont = new ApplicationDbContext();
            ApplicationUser user = (from u in cont.Users
                                    where u.UserName == uname
                                    select u).First();
            return user;
        }


    }
}