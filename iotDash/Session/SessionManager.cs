using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using iotDash.Service;
using iotDbConnector.DAL;
using iotServiceProvider;

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
                    IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                    iotDomain domain = cl.GetDomainWithId(user.DomainId);
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