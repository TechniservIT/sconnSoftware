using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Identity.Roles;
using iotDash.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace iotDash.Controllers.domain.managment.security
{
    public class RoleManagmentController : Controller
    {
        // GET: RoleManagment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateRole(string RoleName, IotUserRole role)
        {
            ApplicationDbContext ucont = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ucont));
            if (!roleManager.RoleExists(RoleName))
            {
                var roleresult2 = roleManager.Create(new IotUserRole(RoleName));
            }

            return View();
        }


    }
}