﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers.domain.surveillance
{
    public class CameraViewController : Controller
    {
        // GET: CameraView
        public ActionResult Index()
        {
            return View();
        }
    }
}