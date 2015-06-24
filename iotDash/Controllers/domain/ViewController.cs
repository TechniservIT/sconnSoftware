using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iotDbConnector.DAL;
using iotServiceProvider;
using iotDeviceService;

namespace iotDash.Controllers
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

                        DeviceRestfulService cl = new DeviceRestfulService();
                        iotDomain domain = cl.GetDomainWithName(appdomain);
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
