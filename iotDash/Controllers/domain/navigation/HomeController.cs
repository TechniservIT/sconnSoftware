using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using iotDash.Models;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Controllers.domain.navigation
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

        [OutputCache(Duration = 100, Location=OutputCacheLocation.ServerAndClient, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 100, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "none")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

         [OutputCache(Duration = 100, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "none")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Front()
         {
             return View();
         }


        [Authorize]
        [OutputCache(Duration = 100, Location = OutputCacheLocation.ServerAndClient, VaryByParam = "none")]
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


                        IIotContextBase dcont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];    //new iotContext();
                        iotDomain domain = dcont.Domains.First(d => d.Id == currentUser.DomainId);
                        if (domain != null)
                        {
                            Session["AppDomain"] = domain.DomainName;
                            return RedirectToAction("Index", "Dashboard", new { app = domain.DomainName });  //userDomain
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
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