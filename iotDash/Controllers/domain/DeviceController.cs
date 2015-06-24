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
				DeviceRestfulService cl = new DeviceRestfulService();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				iotDomain d = cl.DomainWithName(domainId);
				Site toEdit = cl.SiteWithId(Id);  
				DeviceAddModel model = new DeviceAddModel(toEdit,d.Id);
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
				iotDomain domain = cl.DomainWithName(domainId); 
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
				iotDomain d = cl.DomainWithName(domainId);
				Device dev = cl.DeviceAddWithParams(SiteId, Name, Host, Port, Login, Pass, Type, Loc, Prot,d.Id);
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
		public ActionResult View(int DeviceId)
		{
			try
			{
				DeviceRestfulService cl = new DeviceRestfulService();
                Device dev = cl.DeviceWithId(DeviceId);
                if ( dev != null)
                {
                    cl.UpdateDeviceProperties(dev);
                    DeviceViewModel model = new DeviceViewModel(dev);
                    return View(model);
                }
                return View();
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
				List<DeviceAction> actions = cl.DeviceActions().ToList();
				DeviceAction action = (from ac in actions
									   where ac.Id == ActionId
									   select ac).First();
				string currValStr = action.ResultParameters.First().Value;
				action.RequiredParameters.First().Value = currValStr.Equals("1") ? "0" : "1";   //toggle
				action.RequiredParameters.First().Type = action.ResultParameters.First().Type;
				bool result = cl.PerformDeviceAction(action);
				return result == true ? "success" : "error";
			}
			catch (Exception e)
			{
				return "error";
			}

		}
	}
}