using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Service;
using iotDbConnector.DAL;
using iotServiceProvider;

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
				IiotDomainService cl = new iotServiceConnector().GetDomainClient();
				Site toEdit = cl.SiteWithId(Id);
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
				IiotDomainService cl = new iotServiceConnector().GetDomainClient();
				DeviceType type = new DeviceType();
				type.TypeName = Name;
				type.TypeDescription = Description;
				type.VisualRepresentationURL = ImageUrl;
				cl.DeviceTypeAdd(type);
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
				IiotDomainService cl = new iotServiceConnector().GetDomainClient();
				Device dev = cl.DeviceAddWithParams(SiteId, Name, Host, Port, Login, Pass, Type, Loc, Prot);
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
				IiotDomainService cl = new iotServiceConnector().GetDomainClient();
				List<Device> devs = cl.Devices().ToList();
				Device dev = (from d in devs
							  where d.DeviceId == DeviceId
							  select d).First();

				cl.UpdateDeviceProperties(dev); 
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
				IiotDomainService cl = new iotServiceConnector().GetDomainClient();
				List<Device> devs = cl.Devices().ToList();
				Device dev = (from d in devs
							  where d.DeviceId == DeviceId
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
				IiotDomainService cl = new iotServiceConnector().GetDomainClient();
				List<DeviceAction> actions = cl.DeviceActions().ToList();
				DeviceAction action = (from ac in actions
									   where ac.ActionId == ActionId
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