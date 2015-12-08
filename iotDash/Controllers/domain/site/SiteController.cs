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
using iotDash.Content.Dynamic.Status;

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
                iotContext icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; //new iotContext();
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
                iotContext icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; 
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
                iotContext icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; 
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
                iotContext icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; 
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
                iotContext icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; 
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
                    return StatusResponseGenerator.GetSuccessPanelWithMsgAndStat("Location not found.", RequestStatus.Warning);
				}
				 

			}
			catch (Exception e)
			{
                return StatusResponseGenerator.GetSuccessPanelWithMsgAndStat("Add error.", RequestStatus.Failure);
			}

            return StatusResponseGenerator.GetSuccessPanelWithMsgAndStat("Add success.", RequestStatus.Success);
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
                iotContext icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; 
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                d.Sites.Add(site);
                icont.SaveChanges();
			}
			//show status 
			return View();
		}

        // GET: 

        public ActionResult AlarmSystemSummary(int siteId)
        {
            try
            {
                    iotContext cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"]; 
                    Site site = cont.Sites.First(s => s.Id == siteId);
                    if (site != null)
                    {
                        List<Device> alrmSysDevs = site.Devices.Where(d => d.Type.Category == DeviceCategory.AlarmSystem).ToList();
                        if (alrmSysDevs != null)
                        {
                            //AlarmSystemConfigManager mngr = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
                            //int devs = mngr.GetDeviceNumber();
                            AlarmSystemListModel model = new AlarmSystemListModel(alrmSysDevs);
                            return View(model);
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