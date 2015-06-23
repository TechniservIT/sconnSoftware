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
using iotDatabaseConnector.Runtime;

namespace iotDash.Controllers
{

	[DomainAuthorize]
	public class DeviceController : Controller
	{

		//
		// GET: /Device/
		public ActionResult Index()
		{
			//TODO select site
			return View();
		}


		//
		// GET: /Device/Add/<SiteId>
		public ActionResult Add(string SiteId)
		{
			try
			{
				int Id = int.Parse(SiteId);
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				DeviceRestfulService cl = new DeviceRestfulService();
				//iotDomain domain = InMemoryContext.GetDomainForName(domainId);
				Site toEdit = cl.SiteWithId(domainId, Id);  //domain.Sites.First(s => s.Id == Id);  //cl.SiteWithId(domainId, Id);
				DeviceAddModel model = new DeviceAddModel(toEdit,domainId);
				return View(model);
			}
			catch (Exception e)
			{
				return View();
			}
		}


		// GET: /Device/NewType/
		public string NewType(string Name, string Description, string ImageUrl)
		{
			try
			{
				DeviceRestfulService cl = new DeviceRestfulService();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				iotDomain domain = cl.DomainWithId(domainId); //InMemoryContext.GetDomainForName(domainId);
				DeviceType type = new DeviceType();
				type.TypeName = Name;
				type.TypeDescription = Description;
				type.VisualRepresentationURL = ImageUrl;
				//cl.DeviceTypeAdd(type,domainId);
				domain.DeviceTypes.Add(type);
				cl.DomainUpdate(domain);
				return "Add success.";
			}
			catch (Exception e) 
			{
				return "Add error.";
			}          
		}


		 // GET: /Device/AddType/
		public ActionResult AddType()
		{
			return View();
		}
		

		//
		// POST: /Device/New
		public string New(string SiteId, string Name, string Host, string Port, string Login, string Pass, string Type, string Loc, string Prot)
		{
			
			try
			{
				//TODO verify param
				DeviceRestfulService cl = new DeviceRestfulService();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				Device dev = cl.DeviceAddWithParams(SiteId, Name, Host, Port, Login, Pass, Type, Loc, Prot,domainId);
				if (dev != null)
				{
					return "Success";       
				}
				else
				{
					return "Error";       
				}
			}
			catch (Exception e)
			{
				return "Error";        
			}
	  
		}

		// GET: /Device/View/<number>
		public ActionResult View(int DeviceId,int SiteId)
		{
			try
			{
				DeviceRestfulService cl = new DeviceRestfulService();
				List<Device> devs = cl.Devices().ToList();
				Device dev = (from d in devs
							  where d.Id == DeviceId
							  select d).First();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				cl.UpdateDeviceProperties(dev,SiteId,domainId); 
				DeviceViewModel model = new DeviceViewModel(dev);
				return View(model);
			}
			catch (Exception e)
			{
				return View();
			}
		}

		// GET: /Device/Edit/<number>
		public ActionResult Edit(int DeviceId)
		{
			try
			{
				DeviceRestfulService cl = new DeviceRestfulService();
				List<Device> devs = cl.Devices().ToList();
				Device dev = (from d in devs
							  where d.Id == DeviceId
							  select d).First();
				DeviceViewModel model = new DeviceViewModel(dev);
				return View(model);
			}
			catch (Exception e)
			{
				return View();
			}   
		 }



		public string PerformAction(int ActionId, string[] ActionParams)
		{   
			try
			{
				DeviceRestfulService cl = new DeviceRestfulService();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				List<DeviceAction> actions = cl.DeviceActions(domainId).ToList();
				DeviceAction action = (from ac in actions
									   where ac.Id == ActionId
									   select ac).First();
				string currValStr = action.ResultParameters.First().Value;
				action.RequiredParameters.First().Value = currValStr.Equals("1") ? "0" : "1";   //toggle
				action.RequiredParameters.First().Type = action.ResultParameters.First().Type;
				bool result = cl.PerformDeviceAction(action,domainId);
				return result == true ? "success" : "error";
			}
			catch (Exception e)
			{
				return "error";
			}

		}
	}
}