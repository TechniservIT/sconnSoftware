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
using iotDash.Content.Dynamic.Status;

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
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
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
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
				string domainId = DomainSession.GetContextDomain(this.HttpContext);
				iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
				DeviceType type = new DeviceType();
				type.TypeName = Name;
				type.TypeDescription = Description;
				type.VisualRepresentationURL = ImageUrl;
				domain.DeviceTypes.Add(type);
				cont.SaveChanges();
                return StatusResponseGenerator.GetSuccessPanelWithMsgAndStat("Success.", RequestStatus.Success);
			}
			catch (Exception e) 
			{
                return StatusResponseGenerator.GetSuccessPanelWithMsgAndStat("Error.", RequestStatus.Failure);
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
                    var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
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


		 [HttpPost]
        public void Edit(Device Device, int LocationId, int TypeId)    //DeviceEditModel model
		{
			try
			{
                if (Device != null)
				{
                    var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
					DeviceRestfulService cl = new DeviceRestfulService();
                    Device stored = cont.Devices.First(d => d.Id == Device.Id);
					if (stored != null)
					{
                        Location loc = cont.Locations.First(l => l.Id == LocationId);
                        DeviceType type = cont.Types.First(t => t.Id == TypeId);
                        //copy editable objects
                        stored.DeviceName = Device.DeviceName;
                        stored.Type = type;
                        stored.DeviceLocation = loc;
                        stored.Credentials.Username = Device.Credentials.Username;
                        stored.Credentials.Password = Device.Credentials.Password;
                        stored.EndpInfo.Hostname = Device.EndpInfo.Hostname;
                        stored.EndpInfo.Port = Device.EndpInfo.Port;
						cont.SaveChanges();
					}

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
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
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
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
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
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
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
        [HttpGet]
		public ActionResult Edit(int DeviceId)
		{
			try
			{
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
				Device dev = cont.Devices.First(d => d.Id == DeviceId);
				DeviceEditModel model = new DeviceEditModel(dev, cont.Locations.ToList(), cont.Types.ToList() );
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
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
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