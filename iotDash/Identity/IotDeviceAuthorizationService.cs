using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using iotDash.Models;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Identity
{
    public class IotDeviceAuthorizationService
    {
        public async Task<bool> AccessDomainWithCredentials(string domainName, string username, string password)
        {
            try
            {
                var cont = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(cont));
                var user = await UserManager.FindAsync(username, password);
                if (user != null)
                {
                    //store user domain session 
                    var currentUser = (from u in cont.Users
                                       where u.UserName.Equals(username)
                                       select u).First();

                    var icont = UserIotContextFactory.GetContextForUser(currentUser);
                    var domain = icont.Domains.First(dm => dm.Id == currentUser.DomainId);
                    if (domain != null)
                    {
                        string userDomain = domainName;
                        if ((userDomain != null) && !userDomain.Equals(String.Empty))
                        {
                            return true;
                        }
                    }
                    else
                    {
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public bool AccessWithCredentials(string username, string password)
        {
            try
            {
                var cont = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(cont));
                var user =  UserManager.Find(username, password);
                if (user != null)
                {
                    //store user domain session 
                    var currentUser = (from u in cont.Users
                                       where u.UserName.Equals(username)
                                       select u).First();

                    var icont = UserIotContextFactory.GetContextForUser(currentUser);
                    var domain = icont.Domains.First(dm => dm.Id == currentUser.DomainId);
                    if (domain != null)
                    {
                        if ((domain.DomainName != null) && !domain.DomainName.Equals(String.Empty))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }


    }
}