using System;
using System.Linq;
using System.Web.Mvc;
using iotDash.Controllers.domain.navigation;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;

namespace iotDash.Controllers.domain.site.IoT
{
    [DomainAuthorize]
    public class StatisticController : Controller
    {
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PropertyStat(int propertyId)
        {
            var icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
            DeviceProperty prop = icont.Properties.First(p => p.Id == propertyId);
            DevicePropertyStatisticModel model = new DevicePropertyStatisticModel(prop);
            return View(model);
        }

        public ActionResult ActionStat(int actionId)
        {
            var icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
            DeviceAction prop = icont.Actions.First(a => a.Id == actionId);
            DeviceActionStatisticModel model = new DeviceActionStatisticModel(prop);
            return View(model);
        }


        public ActionResult ParameterHistory()
        {
            throw new NotImplementedException();
        }
    }
}