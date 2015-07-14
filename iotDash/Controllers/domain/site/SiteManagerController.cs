using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iotDbConnector.DAL;
using iotServiceProvider;
using iotDash.Session;

namespace iotDash.Controllers
{
    [DomainAuthorize]
    public class SiteManagerController : Controller
    {

        //
        // GET: /SiteManager/
        public ActionResult Index()
        {
            return View();
        }



	}
}