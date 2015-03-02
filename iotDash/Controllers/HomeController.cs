using iotDash.Models;
using iotDash.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Service;
using iotServiceProvider;
using iotDbConnector.DAL;

namespace iotDash.Controllers
{

    public class HomeController : Controller
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public HomeController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [Authorize]
        public ActionResult IOT()
        {
            try
            {
                string userDomain = (string)Session["AppDomain"];
                if ((userDomain != null) && !userDomain.Equals(String.Empty))
                {
                    return RedirectToAction("Index", "Dashboard", new { app = userDomain });
                }
                else
                {
                    //get user domain
                    var user = UserManager.FindById(User.Identity.GetUserId());

                    if (user != null)
                    {
                        //store user domain session 
                        ApplicationDbContext cont = new ApplicationDbContext();
                        var currentUser = (from u in cont.Users
                                           where u.UserName.Equals( user.UserName )
                                           select u).First();

                        IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                        iotDomain domain = cl.GetDomainWithId(currentUser.DomainId);
                        Session["AppDomain"] = domain.DomainName;

                        return RedirectToAction("Index", "Dashboard", new { app = domain.DomainName });  //userDomain
                    }
                    else
                    {
                        //error
                        return RedirectToAction("Login", "Account");
                    }

                } 
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }


    }
}