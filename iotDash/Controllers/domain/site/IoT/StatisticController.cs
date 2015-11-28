using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Models;
using iotDbConnector.DAL;
using iotDash.Session;

namespace iotDash.Controllers
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


    }
}