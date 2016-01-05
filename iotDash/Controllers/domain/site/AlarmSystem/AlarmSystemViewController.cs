using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Controllers.domain.navigation;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
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
    public class AlarmSystemViewController : AlarmSystemControllerBase, IAlarmSystemController
    {
        private DeviceConfigService _provider;

        public AlarmSystemViewController(HttpContextBase contBase) : base(contBase)
        {}


        // GET: AlarmSystemView
        public ActionResult Index(int ServerId)
        {
            try
            {      
               this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
               AlarmSystemDetailModel model = new AlarmSystemDetailModel(this._provider.GetAll(), _provider.Manager.site);
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }


        // GET: AlarmSystemView
        public ActionResult Summary()
        {
            return View();
        }

        // GET: ConfigurationSelect
        public ActionResult ConfigurationSelect(int DeviceId)
        {
            this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemDetailModel model = new AlarmSystemDetailModel(null,_provider.Manager.site);
            model.ServerId = DeviceId;
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


        public ActionResult View(int DeviceId)
        {
            return Index(DeviceId);
        }

    }
}