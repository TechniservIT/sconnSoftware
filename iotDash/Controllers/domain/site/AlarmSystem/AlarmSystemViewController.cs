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
        public ActionResult Index(int ServerId)
        {
            iotContext cont = new iotContext();
            Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
            if (alrmSysDev != null)
            {
                AlarmSystemDetailModel model = new AlarmSystemDetailModel(alrmSysDev);
                return View(model);
            }

            return View();
         
        }

        public ActionResult ToggleArm(int ServerId)
        {
            iotContext cont = new iotContext();
            Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
            if (alrmSysDev != null)
            {
                AlarmSystemDetailModel model = new AlarmSystemDetailModel(alrmSysDev);
                model.Config.ToogleArmStatus();
            }
            return RedirectToAction("Index", new { DeviceId = ServerId });
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



        public ActionResult OutputsConfigure(int ServerId, int AlarmDeviceId)
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    AlarmSystemOutputsConfigureModel model = new AlarmSystemOutputsConfigureModel(alrmSysDev, AlarmDeviceId);
                    return View(model);
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult SaveDeviceOutputs(List<sconnOutput> Outputs, int ServerId, int DeviceId)        //List<sconnInput> Inputs, int DeviceId
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev);
                    if (DeviceId < nm.Config.site.siteCfg.deviceConfigs.Length)
                    {
                        nm.Config.site.siteCfg.deviceConfigs[DeviceId].Outputs = Outputs;
                        nm.Config.site.siteCfg.deviceConfigs[DeviceId].SavePropertiesToRawConfig();
                        nm.Config.StoreDeviceConfig(DeviceId);
                    }

                }
                return RedirectToAction("Index", new { ServerId = ServerId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { ServerId = ServerId });
            }

        }


        public ActionResult InputsConfigure(int ServerId, int AlarmDeviceId)
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    AlarmSystemInputsConfigureModel model = new AlarmSystemInputsConfigureModel(alrmSysDev, AlarmDeviceId);
                     return View(model);
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult SaveDeviceInputs(List<sconnInput> Inputs, int ServerId, int DeviceId)   
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev);
                    if (DeviceId < nm.Config.site.siteCfg.deviceConfigs.Length)
                    {
                        nm.Config.site.siteCfg.deviceConfigs[DeviceId].Inputs = Inputs;
                        nm.Config.site.siteCfg.deviceConfigs[DeviceId].SavePropertiesToRawConfig();
                        nm.Config.StoreDeviceConfig(DeviceId);
                    }

                }
                return RedirectToAction("Index", new { ServerId = ServerId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { ServerId = ServerId });
            }

        }


        public ActionResult RelaysConfigure(int ServerId, int AlarmDeviceId)
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    AlarmSystemRelaysConfigureModel model = new AlarmSystemRelaysConfigureModel(alrmSysDev, AlarmDeviceId);
                    return View(model);
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult SaveDeviceRelays(List<sconnRelay> Relays, int ServerId, int DeviceId)     
        {
            try
            {
                iotContext cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev);
                    if (DeviceId < nm.Config.site.siteCfg.deviceConfigs.Length)
                    {

                    }
                    nm.Config.site.siteCfg.deviceConfigs[DeviceId].Relays = Relays;
                    nm.Config.site.siteCfg.deviceConfigs[DeviceId].SavePropertiesToRawConfig();
                    nm.Config.StoreDeviceConfig(DeviceId);
                }
                return RedirectToAction("Index", new { ServerId = ServerId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { ServerId = ServerId });
            }

        }


    }
}