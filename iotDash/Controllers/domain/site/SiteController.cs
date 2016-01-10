using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.navigation;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using SiteManagmentService;
using iotDash.Identity.Attributes;

namespace iotDash.Controllers.domain.site
{

    [SiteAuthorize]
	public class SiteController : Controller
	{
        private SiteProvider _provider;
        public IIotContextBase Icont { get; set; }

        public SiteController(HttpContextBase contBase)
        {
            try
            {
                DomainSession.LoadDataContextForUserContext(contBase);
                IIotContextBase cont = (IIotContextBase)contBase.Session["iotcontext"];
                this.Icont = cont;
                string domainId = DomainSession.GetContextDomain(contBase);
                iotDomain d = Icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                this._provider = new SiteProvider(this.Icont);
            }
            catch (Exception e)
            {
                    
            }
        }
        
        public ActionResult Index()
		{
			List<Site> sites = new List<Site>();
			try
			{
                sites = this._provider.GetAll();
                ShowSitesViewModel model = new ShowSitesViewModel(sites);
                return View(model);
			}
			catch (Exception e)
			{
                return View();
			}

		}
        
		public bool RemoveDevice(string deviceId)
		{
			int devid = int.Parse(deviceId);    
			try
			{
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
        
		public ActionResult View(int siteId)
		{
			try
			{
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = Icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                DeviceListViewModel model = new DeviceListViewModel(d.Sites.First(s=>s.Id==siteId));
				return View(model);
				
			}
			catch (Exception e)
			{
  
			}
			return View();
		}
        
		public ActionResult Edit(int siteId)
		{
			return View();
		}
        
		public ActionResult Add()
		{          
			try
			{
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = Icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                List<Location> locations = d.Locations.ToList();
                AddSiteViewModel model = new AddSiteViewModel(locations);
                return View(model);
            }
			catch (Exception e)
			{
                return View();
            }
		}


		public string New(string name, string locId)
		{
			try
			{
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = Icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                int locIdnum = int.Parse(locId);
                Location loc = d.Locations.First(l => l.Id == locIdnum);
				if (loc != null)
				{
					Site site = new Site();
					site.SiteName = name;
					site.siteLocation = loc;
					site.Domain = d;
                    Icont.Sites.Add(site);
                    Icont.SaveChanges();
				}
				else
				{
                    return StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Location not found.", RequestStatus.Warning);
				}
				 

			}
			catch (Exception e)
			{
                return StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Add error.", RequestStatus.Failure);
			}

            return StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Add success.", RequestStatus.Success);
		}
        
		public ActionResult Locate(int number)
		{
			return View();
		}
        
		public ActionResult Status(int number)
		{
			return View();
		}
		public ActionResult AddSite(Site site)
		{
			if (site != null)
			{
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = Icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                d.Sites.Add(site);
                Icont.SaveChanges();
			}
			//show status 
			return View();
		}
        

        public ActionResult AlarmSystemSummary(int siteId)
        {
            try
            {
                    Site site = Icont.Sites.First(s => s.Id == siteId);
                    if (site != null)
                    {
                        List<Device> alrmSysDevs = site.Devices.Where(d => d.Type.Category == DeviceCategory.AlarmSystem).ToList();
                        if (alrmSysDevs != null)
                        {
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