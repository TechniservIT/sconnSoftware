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
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemAuthorizedDevicesController : AlarmSystemControllerBase, IAlarmSystemController, IAlarmSystemConfigurationController
    {
        private IAuthorizedDevicesConfigurationService _provider;

        public AlarmSystemAuthorizedDevicesController()
        {
                
        }

        public AlarmSystemAuthorizedDevicesController(HttpContextBase contBase) : base(contBase)
        { }
        
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

        public ActionResult Add()
        {
            AlarmSystemAddAuthorizedDeviceModel model = new AlarmSystemAddAuthorizedDeviceModel();
            return View(model);
        }



        public ActionResult Edit(int Id)
        {
            AlarmSystemAddAuthorizedDeviceModel model = new AlarmSystemAddAuthorizedDeviceModel();
            this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            model.AuthorizedDevice = _provider.GetById(Id);
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(AlarmSystemAddAuthorizedDeviceModel model)
        {
            try
            {
                this._provider = new AuthorizedDevicesConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
                if (ModelState.IsValid)
                {
                    var res = (_provider.Update(model.AuthorizedDevice));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(res);
                }
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetRequestResponseCriticalError();
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