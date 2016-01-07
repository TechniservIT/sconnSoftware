using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using iotDash.Models;
using iotDash.Session;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemEventsController : AlarmSystemControllerBase, IAlarmSystemController, IAlarmSystemConfigurationController
    {
        private IAuthorizedDevicesConfigurationService _provider;

        public AlarmSystemEventsController()
        {

        }

        public AlarmSystemEventsController(HttpContextBase contBase) : base(contBase)
        {

        }

        public ActionResult Search(string key)
        {
            this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemAuthorizedDevicesModel model = new AlarmSystemAuthorizedDevicesModel(_provider.GetAll());
            if (!String.IsNullOrEmpty(key))
            {
                model.AuthorizedDevices = model.AuthorizedDevices.Where(d => d._Serial.Contains(key)).ToList();
            }
            return View(model);
        }
        
        public ActionResult Remove(sconnAuthorizedDevice Device)
        {
            AlarmSystemAddAuthorizedDeviceModel model = new AlarmSystemAddAuthorizedDeviceModel();
            this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            var remRes = this._provider.Remove(Device);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }


        public ActionResult Remove(int Id)
        {
            this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemAuthorizedDevicesModel model = new AlarmSystemAuthorizedDevicesModel(_provider.GetAll());
            bool remRes = _provider.RemoveById(Id);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }


        public ActionResult View(int DeviceId)
        {
            this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemAuthorizedDevicesModel model = new AlarmSystemAuthorizedDevicesModel(this._provider.GetAll());
            return View(model);
        }


    }
}