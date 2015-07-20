using iotDash.Models;
using iotDbConnector.DAL;
using sconnConnector.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iotDash.Controllers
{
    public class AlarmSystemSummaryController : Controller
    {

        // GET: AlarmSystemSummary
        public ActionResult Index(int siteId)
        {
            try
            {
                if (siteId != null)
                {
                    iotContext cont = new iotContext();
                    Site site = cont.Sites.First(s => s.Id == siteId);
                    if (site != null)
                    {
                        Device alrmSysDev = site.Devices.First(d => d.Type.TypeName.Contains("sconnMB"));
                        if (alrmSysDev != null)
                        {
                            AlarmSystemConfigManager mngr = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
                            int devs = mngr.GetDeviceNumber();
                            AlarmSystemSummaryModel model = new AlarmSystemSummaryModel(devs);
                            return View(model);
                        }

                    }
                }

                return View();
            }
            catch (Exception e)
            {
                return View();
            }

        }
    }
}