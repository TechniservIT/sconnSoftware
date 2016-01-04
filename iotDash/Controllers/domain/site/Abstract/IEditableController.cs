using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Content.Dynamic.Status;
using iotDash.Models;
using iotDash.Session;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Controllers.domain.site.Abstract
{
    public interface IEditableController
    {
        ActionResult Search(string key);
        ActionResult Add();
        ActionResult Edit(int Id);
        ActionResult Remove(int Id);
    }

}