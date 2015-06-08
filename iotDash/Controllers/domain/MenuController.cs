using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Top()
        {
            //TODO Session status
            return View();
        }


        public ActionResult Side()
        {
            //TODO load based on user 
            return View();
        }


    }
}