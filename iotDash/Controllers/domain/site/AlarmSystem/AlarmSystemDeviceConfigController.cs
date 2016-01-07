using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Areas.AlarmSystem.Models;
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