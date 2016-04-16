using iotDash.Models;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Identity.Attributes
{
    public class DomainAuthorizeAttribute : AuthorizeAttribute
    {
        public string AppDomain { get; set; }

        private readonly bool _authorize;

        public DomainAuthorizeAttribute()
        {
            _authorize = true;  //auth by default
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!_authorize)
                return true;

            try
            {
                bool basicAuthed = base.AuthorizeCore(httpContext);
                if (basicAuthed)
                {
                    //check domain access
                    string url = httpContext.Request.RawUrl;
                    var urlcomponents = url.Split('/');
                    string appdomain = urlcomponents[1];    //first component after slash
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
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
    }

}