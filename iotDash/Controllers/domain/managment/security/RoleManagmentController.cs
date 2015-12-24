using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.navigation;
using iotDash.Identity.Roles;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace iotDash.Controllers.domain.managment.security
{

    [DomainAuthorize]
    public class RoleManagmentController : Controller
    {

        
        public ActionResult Add()
        {
            try
            {
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
                var model = new IotRoleModel(d);
                return View(model);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }


        public ActionResult View()
        {
            var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
            var nmod = new RoleListModel(d);
            return View(nmod);
        }


        [HttpPost]
        public async Task<ActionResult> Add(IotRoleModel model)
        {
            try
            {
                var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
                var nmod = new IotRoleModel(d);

                if (ModelState.IsValid)
                {
                    ApplicationDbContext ucont = new ApplicationDbContext();
                    var roleManager = new RoleManager<IotUserRole>(new RoleStore<IotUserRole>(ucont));
                    if (!roleManager.RoleExists(model.Role.Name))
                    {
                        await roleManager.CreateAsync(model.Role);
                        nmod.Result = (StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Success.", RequestStatus.Success));
                    }
                    else
                    {
                        nmod.Result = (StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Failure.", RequestStatus.Failure));
                    }
                }
                else
                {
                    nmod.Result = (StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Failure.", RequestStatus.Failure));
                }
                return View(nmod);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }



    }
}