using System;
using System.Linq;
using System.Web.Mvc;
using iotDash.Areas.AlarmSystem.Models;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;

namespace iotDash.Areas.AlarmSystem.Controllers
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
                    IIotContextBase cont = new iotContext();
                    iotDbConnector.DAL.Site site = cont.Sites.First(s => s.Id == siteId);
                    if (site != null)
                    {
                        Device alrmSysDev = site.Devices.First(d => d.Type.TypeName.Contains("sconnMB"));
                        if (alrmSysDev != null)
                        {
                            AlarmSystemConfigManager mngr = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
                            int devs = mngr.GetDeviceNumber();
                            AlarmSystemSummaryModel model = new AlarmSystemSummaryModel(devs, alrmSysDev);
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