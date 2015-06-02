using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDbConnector.DAL;
using System.Web.UI;

namespace iotDash.Controllers
{
    [DomainAuthorize]
    public class MapController : Controller
    {
        //
        // GET: /Map/
        [OutputCache(Duration = 1, Location = OutputCacheLocation.Any, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 1, Location = OutputCacheLocation.Any, VaryByParam = "none")]
        public ActionResult Index(Location loc)
        {
            return View();
        }

        /*******    Display mutiple locations *******/
        public ActionResult Index(List<Location> locations)
        {
            return View();
        }


	}
}