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
using SiteManagmentService;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemAuthorizedDevicesController : Controller
    {
        private IIotContextBase Icont;
        private AuthorizedDevicesConfigurationService _provider;

        public AlarmSystemAuthorizedDevicesController(HttpContextBase contBase)
        {
            IIotContextBase cont = (IIotContextBase)contBase.Session["iotcontext"];
            AlarmSystemConfigManager man = (AlarmSystemConfigManager)contBase.Session["alarmSysCfg"];
            if (cont != null)
            {
                this.Icont = cont;
                string domainId = DomainSession.GetContextDomain(contBase);
                iotDomain d = Icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                this._provider = new AuthorizedDevicesConfigurationService(this.Icont, man);
            }
        }

        // GET: AlarmSystemAuthorizedDevices
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemAddAuthorizedDeviceModel model)
        {
            try
            {
                if (model.AuthorizedDevice != null)
                {
                    _provider.AddAuthorizedDevice(model.AuthorizedDevice);
                    model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Success.",
                        RequestStatus.Success);
                }
                else
                {
                    model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
                }

            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
            }
            return View(model);
        }


    }
}