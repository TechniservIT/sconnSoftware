using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Models;
using iotDbConnector.DAL;
using iotDash.Session;
using iotDeviceService;

namespace iotDash.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PropertyStat(int propertyId, int DeviceId, int SiteId)
        {
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            DeviceRestfulService cl = new DeviceRestfulService();
            DeviceProperty prop = cl.DevicePropertieWithId(propertyId,DeviceId,SiteId, domainId);
            DevicePropertyStatisticModel model = new DevicePropertyStatisticModel(prop);
            return View(model);
        }

        public ActionResult ActionStat(int actionId, int DeviceId, int SiteId)
        {
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            DeviceRestfulService cl = new DeviceRestfulService();
            DeviceAction prop = cl.DeviceActionWithId(actionId, DeviceId, SiteId, domainId);
            DeviceActionStatisticModel model = new DeviceActionStatisticModel(prop);
            return View(model);
        }


    }
}