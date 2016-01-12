using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemDeviceConfigController : AlarmSystemControllerBase, IAlarmSystemController
    {
        private DeviceConfigService _provider;

        public AlarmSystemDeviceConfigController(HttpContextBase contBase) : base(contBase)
        { }

        // GET: AlarmSystemView
        public ActionResult Index(int ServerId)
        {
            try
            {
                var service = new AlarmDevicesConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                AlarmSystemDeviceListModel model = new AlarmSystemDeviceListModel(service.GetAll());
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }
        
        public ActionResult Status(int ServerId, int DeviceId)
        {
            try
            {
                this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId), DeviceId);
                AlarmSystemDeviceModel model = new AlarmSystemDeviceModel(this._provider.Get());
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }


        public ActionResult Edit(int ServerId, int DeviceId)
        {
            try
            {
                this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId), DeviceId);
                AlarmSystemDeviceModel model = new AlarmSystemDeviceModel(this._provider.Get());
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AlarmSystemDeviceModel model)
        {
            try
            {
                this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
                if (ModelState.IsValid)
                {
                    var res = (_provider.Update(model.Device));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(res);
                }
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetRequestResponseCriticalError();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Status(AlarmSystemDeviceModel model)
        {
            try
            {
                this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, model.ServerId), model.Device.DeviceId);
                if (ModelState.IsValid)
                {
                    var res = (_provider.Update(model.Device));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(res);
                }
                return View(model);
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetRequestResponseCriticalError();
            }
            return View(model);
        }


        public string ToggleOutput(string Id)
        {
            try
            {
                int id = int.Parse(Id);
                this._provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
                return _provider.ToggleOutput(id) ? "true" : "false";
            }
            catch (Exception e)
            {
                return "false";
            }
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