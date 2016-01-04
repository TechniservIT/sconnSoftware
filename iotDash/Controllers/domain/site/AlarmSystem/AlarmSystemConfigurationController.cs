using System.Web.Mvc;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemConfigurationController :Controller
    {
        // GET: AlarmSystemConfiguration
        public ActionResult Index()
        {
            return View();
        }
    }
}