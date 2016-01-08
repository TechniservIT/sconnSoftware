using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.navigation;
using iotDash.Controllers.domain.site.AlarmSystem.Abstract;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using iotDash.Identity.Attributes;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    [DomainAuthorize]
    public class AlarmSystemViewController : AlarmSystemControllerBase, IAlarmSystemController
    {
        private GlobalConfigService _provider;
        
        public AlarmSystemViewController(HttpContextBase contBase) : base(contBase)
        {}

        
        public ActionResult Index(int ServerId)
        {
            try
            {
                this._provider = new GlobalConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                AlarmSystemGlobalModel model = new AlarmSystemGlobalModel(this._provider.Get());
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
                this._provider = new GlobalConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, ServerId));
                AlarmSystemGlobalModel model = new AlarmSystemGlobalModel(this._provider.Get());
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

        // GET: ConfigurationSelect
        public ActionResult ConfigurationSelect(int DeviceId)
        {
            AlarmSystemGlobalModel model = new AlarmSystemGlobalModel();
            model.ServerId = DeviceId;
            return View(model);
        }

        // GET: AlarmSystemView
        public ActionResult Device(int DeviceId)
        {
            var provider = new DeviceConfigService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemDetailModel model = new AlarmSystemDetailModel(provider.Get());
            model.ServerId = DeviceId;
            return View(model);
        }
        
        public ActionResult View(int DeviceId)
        {
            return Index(DeviceId);
        }

    }
}