using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers.domain.site.Abstract
{
    public interface IViewAbleController
    {
        ActionResult Index();
    }
}