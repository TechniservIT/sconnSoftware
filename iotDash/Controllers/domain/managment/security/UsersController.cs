using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using iotDash.Content.Dynamic.Status;
using iotDash.Identity.Roles;
using iotDash.Models;
using iotDash.Session;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Controllers.domain.managment.security
{
    [DomainAuthorize]
    public class UsersController : Controller
    {

        //[Authorize(Roles = "DomainAdmin")]
        public ActionResult View()
        {
            try
            {
                var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
                var model = new UserListModel(d);
                return View(model);
            }
            catch (Exception)
            {
                    
                throw;
            }
            return View();
        }


         //[Authorize(Roles = "DomainAdmin")]
        public ActionResult Add()
        {
            var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
            var nmod = new UserCreateModel(d);
            return View(nmod);
        }



         //[Authorize(Roles = "DomainAdmin")]
         [HttpPost]
         public async Task<ActionResult> Add(UserCreateModel model)
         {
             try
             {
                 var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
                 var nmod = new UserCreateModel(d);
                 
                 if (ModelState.IsValid)
                 {
                     ApplicationDbContext ucont = new ApplicationDbContext();
                     var userMan = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                     var result = await userMan.CreateAsync(model.User, model.Password);
                     if (result.Succeeded)
                     {
                         if (model.Roles != null)
                         {
                             foreach (var role in model.Roles)
                             {
                                 userMan.AddToRole(model.User.Id, role.Name);
                             }
                         }
                         nmod.Result = (StatusResponseGenerator.GetPanelWithMsgAndStat("Success.", RequestStatus.Success));
                     }
                     else
                     {
                         nmod.Result = (StatusResponseGenerator.GetPanelWithMsgAndStat("Failure.", RequestStatus.Failure));
                     }
                 }
                 else
                 {
                     nmod.Result = (StatusResponseGenerator.GetPanelWithMsgAndStat("Failure.", RequestStatus.Failure));
                 }
                 return View(nmod);
             }
             catch (Exception e)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
             }
         }





         [Authorize(Roles = "DomainAdmin")]
        public ActionResult Edit(int number)
        {
            return View();
        }




	}
}