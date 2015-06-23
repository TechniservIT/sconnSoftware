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

        public ActionResult PropertyStat(int propertyId)
        {
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            DeviceRestfulService cl = new DeviceRestfulService();
            DeviceProperty prop = cl.DevicePropertieWithId(propertyId,domainId);
            DevicePropertyStatisticModel model = new DevicePropertyStatisticModel(prop);
            return View(model);
        }

        public ActionResult ActionStat(int actionId)
        {
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            DeviceRestfulService cl = new DeviceRestfulService();
            DeviceAction prop = cl.DeviceActionWithId(actionId, domainId);
            DeviceActionStatisticModel model = new DeviceActionStatisticModel(prop);
            return View(model);
        }


    }
}