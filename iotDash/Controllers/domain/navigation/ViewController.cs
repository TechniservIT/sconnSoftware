using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Identity;
using iotDash.Identity.Roles;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Controllers.domain.navigation
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

    static public class DomainAuthHelper
    {
        static public bool IsUserContextDomainAuthorized(HttpContextBase httpContext)
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



    }


    public class SiteAuthorizeAttribute : DomainAuthorizeAttribute
    {
        public string AppDomain { get; set; }

        private readonly bool _authorize;

        public SiteAuthorizeAttribute()
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
                    
                    if (DomainAuthHelper.IsUserContextDomainAuthorized(httpContext))
                    {
                        return DomainAuthHelper.UserHasSiteAccess(httpContext);
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


    public class DeviceAuthorizeAttribute : DomainAuthorizeAttribute
    {
        public string AppDomain { get; set; }

        private readonly bool _authorize;

        public DeviceAuthorizeAttribute()
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
                    if (DomainAuthHelper.IsUserContextDomainAuthorized(httpContext))
                    {
                        return DomainAuthHelper.UserHasDeviceAccess(httpContext);
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



    [DomainAuthorize]
    public class ViewController : Controller
    {
        //
        // GET: /View/
        public ActionResult Index(string app)
        {
            return View();
        }

        //
        // GET: /View/Details/5
        public ActionResult Details(string app, int id)
        {
            return View();
        }

        //
        // GET: /View/Create
        public ActionResult Create(string app)
        {
            return View();
        }

        //
        // POST: /View/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /View/Edit/5
        public ActionResult Edit(string app, int id)
        {
            return View();
        }

        //
        // POST: /View/Edit/5
        [HttpPost]
        public ActionResult Edit(string app, int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /View/Delete/5
        public ActionResult Delete(string app, int id)
        {
            return View();
        }

        //
        // POST: /View/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
