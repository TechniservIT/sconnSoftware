using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using iotDash.Identity.Attributes;
using iotDash.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers.domain.site.AlarmSystem
{

    [DomainAuthorize]
    public class AlarmSystemCommunicationConfigController : AlarmSystemControllerBase, IAlarmSystemController
    {
        private GlobalConfigService _provider;

        public AlarmSystemCommunicationConfigController(HttpContextBase contBase) : base(contBase)
        { }


        public ActionResult Index(int ServerId)
        {
            try
            {
                this._provider = new GlobalConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                AlarmSystemCommunicationEditModel model = new AlarmSystemCommunicationEditModel(this._provider.Get());
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }


        // GET: AlarmSystemView
        public ActionResult Edit(int ServerId)
        {
            try
            {
                var gprovider = new GlobalConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                AlarmSystemCommunicationEditModel model = new AlarmSystemCommunicationEditModel(gprovider.Get());
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }
        
        public ActionResult View(int DeviceId)
        {
            return Index(DeviceId);
        }
        
    }
}