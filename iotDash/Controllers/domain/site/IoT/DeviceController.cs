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
				iotContext cont = new iotContext();
				int Id = int.Parse(SiteId);
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
				Site toEdit = d.Sites.First(s => s.Id == Id);
				DeviceAddModel model = new DeviceAddModel(toEdit);
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
				iotContext cont = new iotContext();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
				DeviceType type = new DeviceType();
				type.TypeName = Name;
				type.TypeDescription = Description;
				type.VisualRepresentationURL = ImageUrl;
				domain.DeviceTypes.Add(type);
				cont.SaveChanges();
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
		// POST: /Device/Create
		
		public void Create(DeviceAddModel model)
		{
			try
			{
				if (model != null)
				{
					iotContext cont = new iotContext();
					DeviceRestfulService cl = new DeviceRestfulService();
					string domainId = DomainSession.GetContextDomain(this.HttpContext);
					iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
					Device dev = cl.DeviceAddWithParamsEntity(
						model.DeviceSiteId.ToString(), 
						model.DeviceName, 
						model.DeviceIpAddr, 
						model.DeviceNetPort.ToString(), 
						model.DeviceLogin, 
						model.DevicePassword, 
						model.DeviceTypeId.ToString(), 
						model.LocationId.ToString(),
						model.DeviceProtocolName, 
						d.Id,
						model.DeviceIsVirtual
						);
				
				}
			}
			catch (Exception e)
			{
				
			}
		}

		//
		// POST: /Device/New
		public string New(string SiteId, string Name, string Host, string Port, string Login, string Pass, string Type, string Loc, string Prot)
		{
			
			try
			{
				//TODO verify param
				iotContext cont = new iotContext();
				DeviceRestfulService cl = new DeviceRestfulService();
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
				Device dev = cl.DeviceAddWithParamsEntity(SiteId, Name, Host, Port, Login, Pass, Type, Loc, Prot,d.Id,false);
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
				iotContext cont = new iotContext();
				DeviceRestfulService cl = new DeviceRestfulService();
				Device dev = cont.Devices.First(d => d.Id == DeviceId);
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


		// GET: /Device/Live/<number>
		public ActionResult Live(int DeviceId)
		{
			try
			{
				iotContext cont = new iotContext();
				DeviceRestfulService cl = new DeviceRestfulService();
				Device dev = cont.Devices.First(d => d.Id == DeviceId);
				if (dev != null)
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
				iotContext cont = new iotContext();
				Device dev = cont.Devices.First(d => d.Id == DeviceId);
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
				iotContext cont = new iotContext();
				DeviceAction action = cont.Actions.First(a => a.Id == ActionId);
				DeviceRestfulService cl = new DeviceRestfulService();
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