using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Content.Dynamic.Status;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using SiteManagmentService;
using AlarmSystemManagmentService.AuthorizedDevices;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemAuthorizedDevicesController : Controller
    {
        private IIotContextBase Icont;
        private IAuthorizedDevicesConfigurationService _provider;

        public AlarmSystemAuthorizedDevicesController(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }
        

        public ActionResult Add()
        {
            AlarmSystemAddAuthorizedDeviceModel model = new AlarmSystemAddAuthorizedDeviceModel();
            return View(model);
        }

        public ActionResult Edit(sconnAuthorizedDevice Device)
        {
            AlarmSystemAddAuthorizedDeviceModel model = new AlarmSystemAddAuthorizedDeviceModel(Device);
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

        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemAddAuthorizedDeviceModel model)
        {
            try
            {
                this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext)); 
                if (ModelState.IsValid)
                {
                    var res = (_provider.Add(model.AuthorizedDevice));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(res);
                }
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetRequestResponseCriticalError();
            }
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