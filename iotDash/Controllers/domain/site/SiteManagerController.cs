using System.Web.Mvc;
using iotDash.Controllers.domain.navigation;

namespace iotDash.Controllers.domain.site
{
    [DomainAuthorize]
    public class SiteManagerController : Controller
    {

        //
        // GET: /SiteManager/
        public ActionResult Index()
        {
            return View();
        }



	}
}