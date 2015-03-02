using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Session;
using System.ServiceModel;
using iotDash.Service;
using iotDbConnector.DAL;
using iotServiceProvider;

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
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                Sites =   cl.Sites().ToList();
            }
            catch (Exception e)
            {

            }
            ShowSitesViewModel model = new ShowSitesViewModel(Sites);
            return View(model);
        }



        //Remove device and return status
        public bool RemoveDevice(string DeviceId)
        {
            int devid = int.Parse(DeviceId);
            try
            {
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                Device dev = cl.DeviceWithId(devid);
                cl.DeviceRemove(dev);
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
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                Site siteToView = cl.SiteWithId(id);
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
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                Locations = cl.Locations().ToList();
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
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                int LocIdnum = int.Parse(LocId);
                Location loc = cl.LocationWithId(LocIdnum);
                if (loc != null)
                {
                    Site site = new Site();
                    site.SiteName = Name;
                    site.siteLocation = loc;
                    iotDomain domain = SessionManager.AppDomainForUserContext(this.HttpContext);
                    iotDomain targetDomain = cl.DomainWithId(domain.DomainId);
                    site.Domain = targetDomain;
                    cl.SiteAdd(site);
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
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                cl.SiteAdd(site);
            }
            //show status 
            return View();
        }

	}
}