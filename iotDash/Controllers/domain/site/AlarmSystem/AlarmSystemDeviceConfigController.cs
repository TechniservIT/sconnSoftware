using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemDeviceConfigController : AlarmSystemControllerBase, IAlarmSystemController
    {
        private DeviceConfigService _provider;

        public AlarmSystemDeviceConfigController(HttpContextBase contBase) : base(contBase)
        { }
        // GET: AlarmSystemView
        public ActionResult Index(int ServerId, int DeviceId)
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
        public async Task<ActionResult> Index(AlarmSystemDeviceModel model)
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