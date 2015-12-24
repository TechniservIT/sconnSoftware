using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iotDash.Content.Dynamic.Status;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemAuthorizedDevicesController : Controller
    {
        // GET: AlarmSystemAuthorizedDevices
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(DeviceAddTypeModel model)
        {
            try
            {
                if (model.Type != null)
                {
                    var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                    string domainId = DomainSession.GetContextDomain(this.HttpContext);
                    iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
                    domain.DeviceTypes.Add(model.Type);
                    await cont.SaveChangesAsync();
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