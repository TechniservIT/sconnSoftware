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
    public class AlarmSystemMapController : AlarmSystemControllerBase, IAlarmSystemController
    {
        private GlobalConfigService _provider;

        public AlarmSystemMapController(HttpContextBase contBase) : base(contBase)
        {
        }

        // GET: AlarmSystemMap
        public ActionResult Index()
        {
            return View();
        }

        // GET: AlarmSystemMap
        public ActionResult View(int Id)
        {
            return View();
        }


        // GET: AlarmSystemMap
        public ActionResult Edit(int ServerId)
        {
            try
            {
                var gprovider = new GlobalConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                var deviceprovider = new AlarmDevicesConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                AlarmSystemMapEditModel model = new AlarmSystemMapEditModel(gprovider.Get(), deviceprovider.GetAll());
                model.ServerId = ServerId;
                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }
    }
}