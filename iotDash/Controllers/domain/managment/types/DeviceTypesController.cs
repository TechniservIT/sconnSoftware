using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iotDash.Content.Dynamic.Status;
using iotDash.Identity.Attributes;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;

namespace iotDash.Controllers.domain.managment.types
{

    [DomainAuthorize]
    public class DeviceTypesController : Controller
    {
        // GET: DeviceTypes
        public ActionResult Index()
        {
            DeviceTypesListModel model = new DeviceTypesListModel();
            try
            {
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
                model = new DeviceTypesListModel(domain.DeviceTypes);
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
            }
            return View(model);
        }

        public ActionResult Add()
        {
            DeviceAddTypeModel model = new DeviceAddTypeModel();
            return View(model);
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

        public ActionResult Edit(int Id)
        {
            var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
            var type = domain.DeviceTypes.FirstOrDefault(t => t.Id == Id);
            DeviceAddTypeModel model = new DeviceAddTypeModel();
            model.Type = type;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(DeviceAddTypeModel model)
        {
            try
            {
                if (model.Type != null)
                {
                    var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                    string domainId = DomainSession.GetContextDomain(this.HttpContext);
                    iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
                    var type = domain.DeviceTypes.FirstOrDefault(t => t.Id == model.Type.Id);
                    type.Load(model.Type);
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

        public ActionResult Remove(int Id)
        {
            return Index();
        }
    }
}