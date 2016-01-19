using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.navigation;
using iotDash.Identity.Roles;
using iotDash.Models;
using iotDash.Session;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using iotDash.Identity.Attributes;

namespace iotDash.Controllers.domain.managment.security
{
    [DomainAuthorize]
    public class UsersController : Controller
    {


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
                                if (role.Active)   //use activation status as select for role
                                {
                                    userMan.AddToRole(model.User.Id, role.Name);
                                }
                             }
                         }
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


        [HttpPost]
        public async Task<ActionResult> Edit(UserCreateModel model)
        {
            try
            {
                var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
                if (ModelState.IsValid)
                {
                    ApplicationDbContext ucont = new ApplicationDbContext();
                    var userMan = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    
                    var user = ucont.Users.FirstOrDefault(u => u.UserName.Equals(model.User.UserName));
                    if (user != null)
                    {
                        String hashedNewPassword = userMan.PasswordHasher.HashPassword(model.Password);
                        UserStore<ApplicationUser> store = new UserStore<ApplicationUser>();
                        await store.SetPasswordHashAsync(user, hashedNewPassword);
                        user.DomainId = model.User.DomainId;
                        user.UserName = model.User.UserName;
                        await ucont.SaveChangesAsync();

                        var userRoles = userMan.GetRoles(model.User.Id);
                        if (model.Roles.Count() != userRoles.Count)
                        {
                            //remove all roles first
                            foreach (var role in userRoles)
                            {
                                userMan.RemoveFromRole(model.User.Id, role);
                            }
                            //add model roles
                            foreach (var role in model.Roles)
                            {
                                if (role.Active)   //use activation status as select for role
                                {
                                    userMan.AddToRole(model.User.Id, role.Name);
                                }
                            }
                        }
                    }

                    var result = await userMan.CreateAsync(model.User, model.Password);
                    if (result.Succeeded)
                    {
                        model.Result = (StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Success.", RequestStatus.Success));
                    }
                    else
                    {
                        model.Result = (StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Failure.", RequestStatus.Failure));
                    }
                }
                else
                {
                    model.Result = (StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Failure.", RequestStatus.Failure));
                }
                return View(model);
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
        
        public ActionResult Index()
        {
            var d = DomainSession.GetDomainForHttpContext(this.HttpContext);
            var model = new UserListModel(d);
            return View(model);
        }

        public ActionResult Remove(int Id)
        {
            return Index();
        }
    }
}