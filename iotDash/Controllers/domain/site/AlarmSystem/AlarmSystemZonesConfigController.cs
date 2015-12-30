using System;
using System.Collections.Generic;
using System.Linq;
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
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemZonesConfigController : Controller
    {
        private IIotContextBase Icont;
        private ZoneConfigurationService _provider;

        public AlarmSystemZonesConfigController(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }

        
        public ActionResult View(int DeviceId)
        {
            this._provider = new ZoneConfigurationService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemZoneConfigModel model = new AlarmSystemZoneConfigModel(this._provider.GetAll());
            return View(model);
        }


        public ActionResult Edit(sconnAlarmZone Zone)
        {
            AlarmSystemZoneAddModel model = new AlarmSystemZoneAddModel(Zone);
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            AlarmSystemZoneAddModel model = new AlarmSystemZoneAddModel();
            this._provider = new ZoneConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            model.Zone = _provider.GetById(Id);
            return View(model);
        }

        public ActionResult Search(string key)
        {
            this._provider = new ZoneConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemZoneConfigModel model = new AlarmSystemZoneConfigModel(_provider.GetAll());
            if (!String.IsNullOrEmpty(key))
            {
                model.ZoneConfigs = model.ZoneConfigs.Where(d => d.Name.Contains(key)).ToList();
            }
            return View(model);
        }

        public ActionResult Remove(int Id)
        {
            this._provider = new ZoneConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemZoneConfigModel model = new AlarmSystemZoneConfigModel(_provider.GetAll());
            bool remRes = _provider.RemoveById(Id);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }

        public ActionResult Remove(sconnAlarmZone Zone)
        {
            AlarmSystemZoneAddModel model = new AlarmSystemZoneAddModel();
            this._provider = new ZoneConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            var remRes = this._provider.Remove(Zone);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }

        public ActionResult Add()
        {
            AlarmSystemZoneAddModel model = new AlarmSystemZoneAddModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemZoneAddModel model)
        {
            try
            {
                this._provider = new ZoneConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
                if (ModelState.IsValid)
                {
                    var res = (_provider.Add(model.Zone));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(res);
                }
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetRequestResponseCriticalError();
            }
            return View(model);
        }

    }
}