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
            DeviceRestfulService cl = new DeviceRestfulService();
            DeviceProperty prop = cl.DevicePropertieWithId(propertyId);
            DevicePropertyStatisticModel model = new DevicePropertyStatisticModel(prop);
            return View(model);
        }

        public ActionResult ActionStat(int actionId)
        {
            DeviceRestfulService cl = new DeviceRestfulService();
            DeviceAction prop = cl.DeviceActionWithId(actionId);
            DeviceActionStatisticModel model = new DeviceActionStatisticModel(prop);
            return View(model);
        }


    }
}