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
using sconnConnector.Config;

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
                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                Sites = d.Sites.ToList();
                ShowSitesViewModel model = new ShowSitesViewModel(Sites);
                return View(model);
			}
			catch (Exception e)
			{
                return View();
			}

		}



		//Remove device and return status
		public bool RemoveDevice(string DeviceId)
		{
			int devid = int.Parse(DeviceId);    
			try
			{
                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                Device dev = icont.Devices.First(s => s.Id == devid);
                icont.Devices.Remove(dev);
                icont.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}


		//
		// GET: /Site/View/<number>
		public ActionResult View(int SiteId)
		{
			try
			{
                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                DeviceListViewModel model = new DeviceListViewModel(d.Sites.First(s=>s.Id==SiteId));
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
			try
			{
                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                List<Location> Locations = d.Locations.ToList();
                AddSiteViewModel model = new AddSiteViewModel(Locations);
                return View(model);
            }
			catch (Exception e)
			{
                return View();
            }
		}

		//
		// GET: /Site/New
		public string New(string Name, string LocId)
		{
			try
			{
                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                int LocIdnum = int.Parse(LocId);
                Location loc = d.Locations.First(l => l.Id == LocIdnum);
				if (loc != null)
				{
					Site site = new Site();
					site.SiteName = Name;
					site.siteLocation = loc;
					site.Domain = d;
                    icont.Sites.Add(site);
                    icont.SaveChanges();
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
                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                d.Sites.Add(site);
                icont.SaveChanges();
			}
			//show status 
			return View();
		}

        // GET: AlarmSystemSummary
        public ActionResult AlarmSystemSummary(int siteId)
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