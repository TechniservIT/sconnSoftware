using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Event;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using iotDash.Models;
using iotDash.Session;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemEventsController : AlarmSystemControllerBase, IAlarmSystemController, IAlarmSystemConfigurationController
    {
        private EventsService _provider;

        public AlarmSystemEventsController()
        {

        }

        public AlarmSystemEventsController(HttpContextBase contBase) : base(contBase)
        {

        }

        public ActionResult Search(string key)
        {
            this._provider = new EventsService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemEventsModel model = new AlarmSystemEventsModel(_provider.GetAll());
            if (!String.IsNullOrEmpty(key))
            {
                model.Events = model.Events.Where(d => d.Type.ToString().Contains(key)).ToList();
            }
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            return View();
        }

        public ActionResult Remove(sconnEvent Device)
        {
            AlarmSystemAddAuthorizedDeviceModel model = new AlarmSystemAddAuthorizedDeviceModel();
            this._provider = new EventsService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            var remRes = this._provider.Remove(Device);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }


        public ActionResult Remove(int Id)
        {
            this._provider = new EventsService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemEventsModel model = new AlarmSystemEventsModel(_provider.GetAll());
            bool remRes = _provider.RemoveById(Id);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }


        public ActionResult View(int DeviceId)
        {
            this._provider = new EventsService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemEventsModel model = new AlarmSystemEventsModel(this._provider.GetAll());
            return View(model);
        }


    }
}