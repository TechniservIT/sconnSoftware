using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using iotDash.Controllers.domain.navigation;
using iotDbConnector.DAL;
using iotDash.Identity.Attributes;

namespace iotDash.Controllers.domain.site
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