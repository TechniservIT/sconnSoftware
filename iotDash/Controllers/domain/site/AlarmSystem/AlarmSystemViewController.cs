using iotDash.Models;
using iotDbConnector.DAL;
using sconnConnector.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers
{
    public class AlarmSystemViewController : Controller
    {
        // GET: AlarmSystemView
        public ActionResult Index(int DeviceId)
        {
            iotContext cont = new iotContext();
            Device alrmSysDev = cont.Devices.First(d => d.Id == DeviceId);
            if (alrmSysDev != null)
            {
                AlarmSystemDetailModel model = new AlarmSystemDetailModel(alrmSysDev);
                return View(model);
            }

            return View();
        }

        // GET: AlarmSystemView
        public ActionResult Summary()
        {
            return View();
        }


        // GET: AlarmSystemView
        public ActionResult Device(int DeviceId)
        {
            return View();
        }


    }
}