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

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemZonesConfigController : Controller
    {
        private IIotContextBase Icont;
        private ZoneConfigurationService _provider;

        public AlarmSystemZonesConfigController(HttpContextBase contBase)
        {
            DomainSession.LoadDataContextForUserContext(contBase);
            IIotContextBase cont = (IIotContextBase)contBase.Session["iotcontext"];
            Device alrmSysDev = (Device)contBase.Session["alarmDevice"];
            if (alrmSysDev != null)
            {
                AlarmSystemConfigManager man = DomainSession.GetAlarmConfigForContextWithDevice(contBase,
                    alrmSysDev);
                this._provider = new ZoneConfigurationService(this.Icont, man);
            }
            this.Icont = cont;
        }

        private void LoadAlarmSystemService(int DevId)
        {
            Device alrmSysDev = (Device)this.HttpContext.Session["alarmDevice"];
            if (alrmSysDev != null)
            {
                AlarmSystemConfigManager man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext,
                    alrmSysDev);
                this._provider = new ZoneConfigurationService(this.Icont, man);
            }
            else
            {
                alrmSysDev = Icont.Devices.First(d => d.Id == DevId);
                if (alrmSysDev != null)
                {
                    this.HttpContext.Session["alarmDevice"] = alrmSysDev;
                    AlarmSystemConfigManager man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext,
                        alrmSysDev);
                    this._provider = new ZoneConfigurationService(this.Icont, man);
                }
            }
        }

        public ActionResult View(int DeviceId)
        {
            LoadAlarmSystemService(DeviceId);
            AlarmSystemZoneConfigModel model = new AlarmSystemZoneConfigModel(this._provider.GetAlarmZoneConfig());
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
                if (ModelState.IsValid)
                {
                    var res = (_provider.AddZone(model.Zone));
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