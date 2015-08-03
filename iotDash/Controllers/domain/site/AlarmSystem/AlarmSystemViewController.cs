using iotDash.Models;
using iotDbConnector.DAL;
using sconnConnector;
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

        public ActionResult ToggleArm(int DeviceId)
        {
            iotContext cont = new iotContext();
            Device alrmSysDev = cont.Devices.First(d => d.Id == DeviceId);
            if (alrmSysDev != null)
            {
                AlarmSystemDetailModel model = new AlarmSystemDetailModel(alrmSysDev);
                model.Config.ToogleArmStatus();
            }
            return RedirectToAction("Index", new { DeviceId = DeviceId });
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


        public ActionResult SaveDevice(List<sconnInput> Inputs, int DeviceId)        //List<sconnInput> Inputs, int DeviceId
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == DeviceId);
                if (alrmSysDev != null)
                {
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev);
                    nm.Config.site.siteCfg.deviceConfigs[0].Inputs = Inputs;
                    nm.Config.site.siteCfg.deviceConfigs[0].SavePropertiesToRawConfig();
                    nm.Config.StoreDeviceConfig(0);
                }
                return RedirectToAction("Index", new { DeviceId = DeviceId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { DeviceId = DeviceId });
            }

        }


    }
}