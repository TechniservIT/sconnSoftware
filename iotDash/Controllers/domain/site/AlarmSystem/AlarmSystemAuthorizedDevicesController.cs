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

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemAuthorizedDevicesController : Controller
    {
        private IIotContextBase Icont;
        private AuthorizedDevicesConfigurationService _provider;

        public AlarmSystemAuthorizedDevicesController(HttpContextBase contBase)
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
                this._provider = new AuthorizedDevicesConfigurationService(this.Icont, man);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemAddAuthorizedDeviceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = (_provider.AddAuthorizedDevice(model.AuthorizedDevice));
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
            LoadAlarmSystemService(DeviceId);
            AlarmSystemAuthorizedDevicesModel model = new AlarmSystemAuthorizedDevicesModel(this._provider.GetAuthorizedDevices());
            return View(model);
        }

    }
}