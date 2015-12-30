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
    public class AlarmSystemGsmConfigController : Controller
    {
        private IIotContextBase Icont;
        private GsmConfigurationService _provider;

        public AlarmSystemGsmConfigController(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }

       

        public ActionResult View(int DeviceId)
        {
            this._provider = new GsmConfigurationService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemGsmConfigModel model = new AlarmSystemGsmConfigModel(this._provider.GetAll());
            return View(model);
        }

        public ActionResult Add()
        {
            AlarmSystemGsmAddRcptModel model = new AlarmSystemGsmAddRcptModel();
            return View(model);
        }

        public ActionResult Edit(sconnGsmRcpt Rcpt)
        {
            AlarmSystemGsmAddRcptModel model = new AlarmSystemGsmAddRcptModel(Rcpt);
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            AlarmSystemGsmAddRcptModel model = new AlarmSystemGsmAddRcptModel();
            this._provider = new GsmConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            model.GsmRcpt = _provider.GetById(Id);
            return View(model);
        }
        
        public ActionResult Search(string key)
        {
            this._provider = new GsmConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemGsmConfigModel model = new AlarmSystemGsmConfigModel(_provider.GetAll());
            if (!String.IsNullOrEmpty(key))
            {
                model.GsmRcpts = model.GsmRcpts.Where(d => d.NumberE164.Contains(key) || d.Name.Contains(key)).ToList();
            }
            return View(model);
        }

        public ActionResult Remove(int Id)
        {
            this._provider = new GsmConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemGsmConfigModel model = new AlarmSystemGsmConfigModel(_provider.GetAll());
            bool remRes = _provider.RemoveById(Id);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }


        public ActionResult Remove(sconnGsmRcpt Rcpt)
        {
            AlarmSystemGsmAddRcptModel model = new AlarmSystemGsmAddRcptModel();
            this._provider = new GsmConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            var remRes = this._provider.Remove(Rcpt);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }

        
        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemGsmAddRcptModel model)
        {
            try
            {
                this._provider = new GsmConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
                if (ModelState.IsValid)
                {
                    var res = (_provider.Add(model.GsmRcpt));
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