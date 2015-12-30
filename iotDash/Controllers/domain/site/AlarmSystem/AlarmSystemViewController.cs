using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Controllers.domain.navigation;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using iotDash.Identity.Attributes;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    [DomainAuthorize]
    public class AlarmSystemViewController : Controller
    {
        private IIotContextBase Icont;
        private DeviceConfigService _provider;

        public AlarmSystemViewController(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }


        // GET: AlarmSystemView
        public ActionResult Index(int ServerId)
        {
            try
            {      
                    this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                    AlarmSystemDetailModel model = new AlarmSystemDetailModel(this._provider.GetAll(), _provider.Manager.site);
                    return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }

        //TODO
        //public ActionResult ToggleArm(int ServerId)
        //{
        //    try
        //    {
        //        Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
        //        if (alrmSysDev != null)
        //        {
        //            var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
        //            AlarmSystemDetailModel model = new AlarmSystemDetailModel(alrmSysDev, man);
        //            model.Config.ToogleArmStatus();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //err msg
        //    }
        //    return RedirectToAction("Index", new { DeviceId = ServerId });
        //}


        // GET: AlarmSystemView
        public ActionResult Summary()
        {
            return View();
        }

        // GET: ConfigurationSelect
        public ActionResult ConfigurationSelect(int DeviceId)
        {
            this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemDetailModel model = new AlarmSystemDetailModel(this._provider.GetAll(),_provider.Manager.site);
            return View(model);
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
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemOutputsConfigureModel model = new AlarmSystemOutputsConfigureModel(alrmSysDev, AlarmDeviceId, man);
                    return View(model);
                }
            }
            catch (Exception e)
            {
               
            }
            return View();
        }
        

        public ActionResult InputsConfigure(int ServerId, int AlarmDeviceId)
        {
            try
            {
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemInputsConfigureModel model = new AlarmSystemInputsConfigureModel(alrmSysDev, AlarmDeviceId, man);
                     return View(model);
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }
        
        public ActionResult RelaysConfigure(int ServerId, int AlarmDeviceId)
        {
            try
            {
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemRelaysConfigureModel model = new AlarmSystemRelaysConfigureModel(alrmSysDev, AlarmDeviceId, man);
                    return View(model);
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }


    }
}