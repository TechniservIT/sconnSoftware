using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iotDash.Controllers.domain.site.Abstract;
using iotDash.Models;

namespace iotDash.Controllers.domain.site.AlarmSystem.Abstract
{
    public interface IAlarmSystemConfigurationController : IEditableController, IAlarmSystemController
    {
        ActionResult View(int DeviceId);
    }

}