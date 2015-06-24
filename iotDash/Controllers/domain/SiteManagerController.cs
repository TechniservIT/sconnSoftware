using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using iotDbConnector.DAL;
using iotServiceProvider;
using iotDeviceService;
using iotDash.Session;

namespace iotDash.Controllers
{
    [DomainAuthorize]
    public class SiteManagerController : Controller
    {
        public List<Site> Sites { get; set; }

        //
        // GET: /SiteManager/
        public ActionResult Index()
        {
            //load sites
            DeviceRestfulService serv = new DeviceRestfulService();
            string domainName = DomainSession.GetContextDomain(this.HttpContext);
            iotDomain domain = serv.GetDomainWithName(domainName);
            var sites = serv.GetSitesInDomain(domain.Id).ToList();
            foreach (Site site in sites)
            {
                Sites.Add(site); 
            }

            return View();
        }


        // GET: /SiteManager/brief
        // short sites detail
        public ActionResult brief()
        {
            return View();
        }


        // GET: /SiteManager/search
        public ActionResult search()
        {
            return View();
        }



	}
}