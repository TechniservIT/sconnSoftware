using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.navigation;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using iotDash.Identity.Attributes;

namespace iotDash.Controllers.domain.site
{
	[DomainAuthorize]

	public class LocationController : Controller
	{
		//
		// GET: /Location/
		[OutputCache(Duration = 10, Location = OutputCacheLocation.Any, VaryByParam = "none")]
		public ActionResult Index()
		{
            IIotContextBase icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
            List<Location> locs = d.Locations.ToList();
            LocationListViewModel model = new LocationListViewModel(locs);
			return View(model);
		}

		public string New(string Name, string Lat, string Lng)
		{
			try
			{
				
				string username = User.Identity.Name;
				ApplicationDbContext cont = new ApplicationDbContext();
				var user = (from u in cont.Users
							where u.UserName == username
							select u).First();


                IIotContextBase icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));

                Location loc = new Location();
				loc.Domain = d;
				loc.LocationName = Name;
				loc.Lng = double.Parse(Lng, CultureInfo.InvariantCulture);
				loc.Lat = double.Parse(Lat, CultureInfo.InvariantCulture);
				d.Locations.Add(loc);
                icont.SaveChanges();
                return StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Location added sucessfully.", RequestStatus.Success);
			}
			catch (Exception e)
			{
                return StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Location add failed.", RequestStatus.Failure);
			}
		}


		public ActionResult Add()
		{
            LocatioAddModel model = new LocatioAddModel();
            return View(model);
		}
		public ActionResult Edit(string LocationId)
		{
			try
			{
				int LocId = int.Parse(LocationId);
                IIotContextBase icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                Location loc = d.Locations.First(l => l.Id == LocId);
				if (loc != null)
				{
					LocationEditViewModel model = new LocationEditViewModel(loc);
					return View(model);
				}     
			}
			catch (Exception e)
			{
			}
			return View();
		}

		public bool Remove(string LocationId)
		{
			try
			{
				int LocId = int.Parse(LocationId);
                IIotContextBase icont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));
                Location loc = d.Locations.First(l => l.Id == LocId);
                d.Locations.Remove(loc);
                icont.SaveChanges();
                return true;     
			}
			catch (Exception e)
			{
				return false;
			}      
		}


	    public ActionResult Search()
	    {
	        throw new NotImplementedException();
	    }
	}
}