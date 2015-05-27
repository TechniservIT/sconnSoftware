using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDbConnector.DAL;

namespace iotDash.Controllers
{
    [DomainAuthorize]
    public class MapController : Controller
    {
        //
        // GET: /Map/
        public ActionResult Index()
        {
            return View();
        }

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