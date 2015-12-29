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
        private GlobalConfigService _provider;
        private AlarmSystemDetailModel _model;

        public AlarmSystemViewController(HttpContextBase contBase)
        {
            DomainSession.LoadDataContextForUserContext(contBase);
            IIotContextBase cont = (IIotContextBase)contBase.Session["iotcontext"];
            this.Icont = cont;
        }

        private void LoadAlarmSystemService(int DevId)
        {
            Device alrmSysDev = Icont.Devices.First(d => d.Id == DevId);
            if (alrmSysDev != null)
            {
                AlarmSystemConfigManager man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext,
                    alrmSysDev);
                this._provider = new GlobalConfigService(this.Icont, man);
            }
        }

        // GET: AlarmSystemView
        public ActionResult Index(int ServerId)
        {
            try
            {      
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    LoadAlarmSystemService(alrmSysDev.Id);
                    _model = new AlarmSystemDetailModel(alrmSysDev, man);
                    return View(_model);
                }
            }
            catch (Exception e)
            {
            }
            return View();
        }

        public ActionResult ToggleArm(int ServerId)
        {
            try
            {
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemDetailModel model = new AlarmSystemDetailModel(alrmSysDev, man);
                    model.Config.ToogleArmStatus();
                }
            }
            catch (Exception e)
            {
                //err msg
            }
            return RedirectToAction("Index", new { DeviceId = ServerId });
        }


        // GET: AlarmSystemView
        public ActionResult Summary()
        {
            return View();
        }

        // GET: ConfigurationSelect
        public ActionResult ConfigurationSelect(int DeviceId)
        {
            Device alrmSysDev = Icont.Devices.First(d => d.Id == DeviceId);
            if (alrmSysDev != null)
            {
                var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                LoadAlarmSystemService(alrmSysDev.Id);
                _model = new AlarmSystemDetailModel(alrmSysDev, man);
                return View(_model);
            }
            return View(_model);
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

        public ActionResult SaveDeviceOutputs(List<sconnOutput> Outputs, int ServerId, int DeviceId)
        {
            try
            {
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev, man);
                    if (DeviceId < nm.Config.site.siteCfg.deviceConfigs.Length)
                    {
                        nm.Config.site.siteCfg.deviceConfigs[DeviceId].Outputs = Outputs;
                        nm.Config.site.siteCfg.deviceConfigs[DeviceId].SavePropertiesToRawConfig();
                        nm.Config.StoreDeviceConfig(DeviceId);
                    }

                }
            }
            catch (Exception e)
            {
                //err msg
            }
            return RedirectToAction("Index", new { ServerId = ServerId });
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

        public ActionResult SaveDeviceInputs(List<sconnInput> Inputs, int ServerId, int DeviceId)   
        {
            try
            {
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev, man);
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

        public ActionResult SaveDeviceRelays(List<sconnRelay> Relays, int ServerId, int DeviceId)     
        {
            try
            {
                Device alrmSysDev = Icont.Devices.First(d => d.Id == ServerId);
                if (alrmSysDev != null)
                {
                    var man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext, alrmSysDev);
                    AlarmSystemDetailModel nm = new AlarmSystemDetailModel(alrmSysDev, man);
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