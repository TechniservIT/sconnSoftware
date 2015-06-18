using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Session;
using System.ServiceModel;
 
using iotDbConnector.DAL;
using iotServiceProvider;
using iotDeviceService;

namespace iotDash.Controllers
{

    [DomainAuthorize]
    public class SiteController : Controller
    {
        public Site site { get; set; }

        //
        // GET: /Site/
        //  Display site list

        public ActionResult Index()
        {
            List<Site> Sites = new List<Site>();
            try
            {
                DeviceRestfulService cl = new DeviceRestfulService();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                Sites = cl.GetSitesInDomain(domainId).ToList();
            }
            catch (Exception e)
            {

            }
            ShowSitesViewModel model = new ShowSitesViewModel(Sites);
            return View(model);
        }



        //Remove device and return status
        public bool RemoveDevice(string DeviceId, string SiteId)
        {
            int devid = int.Parse(DeviceId);
            int sid = int.Parse(SiteId);
            try
            {
                DeviceRestfulService cl = new DeviceRestfulService();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                Device dev = cl.DeviceWithId(devid, sid, domainId);
                cl.DeviceRemove(dev,sid,domainId);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        //
        // GET: /Site/View/<number>
        public ActionResult View(string SiteId)
        {
            int id = int.Parse(SiteId);
            try
            {
                DeviceRestfulService cl = new DeviceRestfulService();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                Site siteToView = cl.SiteWithId(domainId,id);
                DeviceListViewModel model = new DeviceListViewModel(siteToView);
                return View(model);
                
            }
            catch (Exception e)
            {
  
            }
            return View();
        }

        //
        // GET: /Site/Edit/<number>
        public ActionResult Edit(int SiteId)
        {
            return View();
        }





        //
        // GET: /Site/Add
        public ActionResult Add()
        {          
            List<Location> Locations = new List<Location>();
            try
            {
                DeviceRestfulService cl = new DeviceRestfulService();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                Locations = cl.Locations(domainId).ToList();
            }
            catch (Exception e)
            {

            }
            AddSiteViewModel model = new AddSiteViewModel(Locations);
            return View(model);
        }

        //
        // GET: /Site/New
        public string New(string Name, string LocId)
        {
            try
            {
                DeviceRestfulService cl = new DeviceRestfulService();
                int LocIdnum = int.Parse(LocId);
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                Location loc = cl.LocationWithId(LocIdnum,domainId);
                if (loc != null)
                {
                    Site site = new Site();
                    site.SiteName = Name;
                    site.siteLocation = loc;
                    iotDomain domain = SessionManager.AppDomainForUserContext(this.HttpContext);
                    iotDomain targetDomain = cl.DomainWithId(domain.DomainName);
                    site.Domain = targetDomain;
                    cl.AddSiteToDomain(site,domain.DomainName);
                }
                else
                {
                    return "Location not found";
                }
                 

            }
            catch (Exception e)
            {
                return "Add error.";
            }

            return "Add success.";
        }




        //
        // GET: /Site/Locate/<number>
        public ActionResult Locate(int number)
        {
            return View();
        }


        //
        // GET: /Site/Status/<number>
        public ActionResult Status(int number)
        {
            return View();
        }
        public ActionResult AddSite(Site site)
        {
            if (site != null)
            {
                DeviceRestfulService cl = new DeviceRestfulService();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                cl.AddSiteToDomain(site,domainId);
            }
            //show status 
            return View();
        }

	}
}