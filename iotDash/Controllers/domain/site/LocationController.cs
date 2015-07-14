using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;
using iotServiceProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace iotDash.Controllers
{
	[DomainAuthorize]
	public class LocationController : Controller
	{
		//
		// GET: /Location/
		[OutputCache(Duration = 10, Location = OutputCacheLocation.Any, VaryByParam = "none")]
		public ActionResult Index()
		{
            iotContext cont = new iotContext();
            string domainId = DomainSession.GetContextDomain(this.HttpContext);
            iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
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


                iotContext icont = new iotContext();
                string domainId = DomainSession.GetContextDomain(this.HttpContext);
                iotDomain d = icont.Domains.First(dm => dm.DomainName.Equals(domainId));

                Location loc = new Location();
				loc.Domain = d;
				loc.LocationName = Name;
				loc.Lng = double.Parse(Lng, CultureInfo.InvariantCulture);
				loc.Lat = double.Parse(Lat, CultureInfo.InvariantCulture);
				d.Locations.Add(loc);
                icont.SaveChanges();
				return "Location added sucessfully";
			}
			catch (Exception e)
			{
				return "Location add failed";
			}
		}


		public ActionResult Add()
		{
			return View();
		}
		public ActionResult Edit(string LocationId)
		{
			try
			{
				int LocId = int.Parse(LocationId);
                iotContext icont = new iotContext();
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
                iotContext icont = new iotContext();
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


	}
}