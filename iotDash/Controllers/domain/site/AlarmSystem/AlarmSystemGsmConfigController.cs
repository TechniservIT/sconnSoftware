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
    public class AlarmSystemGsmConfigController : Controller
    {
        private IIotContextBase Icont;
        private GsmConfigurationService _provider;

        public AlarmSystemGsmConfigController(HttpContextBase contBase)
        {
            DomainSession.LoadDataContextForUserContext(contBase);
            IIotContextBase cont = (IIotContextBase)contBase.Session["iotcontext"];
            this.Icont = cont;
        }

        private void LoadAlarmSystemService(int DevId)
        {
            Device alrmSysDev = Icont.Devices.First(d => d.Id == DevId);
            if (alrmSysDev != null)
            {
                AlarmSystemConfigManager man = DomainSession.GetAlarmConfigForContextWithDevice(this.HttpContext,
                    alrmSysDev);
                this._provider = new GsmConfigurationService(man);
            }
        }

        public ActionResult View(int DeviceId)
        {
            LoadAlarmSystemService(DeviceId);
            AlarmSystemGsmConfigModel model = new AlarmSystemGsmConfigModel(this._provider.GetAll());
            return View(model);
        }

        public ActionResult Add()
        {
            AlarmSystemGsmAddRcptModel model = new AlarmSystemGsmAddRcptModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemGsmAddRcptModel model)
        {
            try
            {
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