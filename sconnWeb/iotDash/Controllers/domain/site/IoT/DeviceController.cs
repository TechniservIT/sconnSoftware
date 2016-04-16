using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeviceManagmentService;
using iotDash.Content.Dynamic.Status;
using iotDash.Controllers.domain.navigation;
using iotDash.Controllers.domain.site.Abstract;
using iotDash.Models;
using iotDash.Session;
using iotDbConnector.DAL;
using iotDash.Identity.Attributes;
using SiteManagmentService;

namespace iotDash.Controllers.domain.site.IoT
{

	[DeviceAuthorize]
	public class DeviceController : EntityControllerBase 
	{
        public DeviceController(HttpContextBase contBase) : base(contBase)
        { }


		//
		// GET: /Device/Add/<SiteId>
		public ActionResult Add(int SiteId)
		{
			try
			{
                var provider = new SiteProvider(DomainSession.GetDataContextForUserContext(this.HttpContext));
                DeviceAddModel model = new DeviceAddModel(provider.GetById(SiteId));
                return View(model);
			}
			catch (Exception e)
			{
				return View();
			}
		}

	    [HttpGet]
	    public ActionResult AddType()
	    {
            DeviceAddTypeModel model = new DeviceAddTypeModel();
	        return View(model);
	    }

	    [HttpPost]
	    public async Task<ActionResult> AddType(DeviceAddTypeModel model)
	    {
            try
            {
                if (model.Type != null)
                {
                    var cont = (iotContext) System.Web.HttpContext.Current.Session["iotcontext"];
                    string domainId = DomainSession.GetContextDomain(this.HttpContext);
                    iotDomain domain = cont.Domains.First(d => d.DomainName.Equals(domainId));
                    domain.DeviceTypes.Add(model.Type);
                    await cont.SaveChangesAsync();
                    model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Success.",
                        RequestStatus.Success);
                }
                else
                {
                    model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
                }

            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
            }
            return View(model);
	    }



		// GET: /Device/NewType/
		public ActionResult NewType(string Name, string Description, string ImageUrl)
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
			    return View();  //StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Success.", RequestStatus.Success);
			}
			catch (Exception e) 
			{
                return View();  //return StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
			}
		  
		}


        //
        // POST: /Device/Create
        [HttpPost]
        public async Task<ActionResult> Add(DeviceAddModel model)
		{
			try
			{
                var provider = new DeviceProvider(DomainSession.GetDataContextForUserContext(this.HttpContext));
                model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(provider.Add(model.Device,model.LocationId,model.TypeId,model.DeviceSiteId));
                return View(model);
            }
			catch (Exception e)
			{
                return View();
            }
		}
        
        [HttpPost]
        public async Task<ActionResult> Edit(DeviceEditModel model)
		{
			try
			{
			    if (model.Device != null)
			    {
                    var provider = new DeviceProvider(DomainSession.GetDataContextForUserContext(this.HttpContext));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(provider.Update(model.Device));
                    return View(model);
                }
			    else
			    {
                    model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
			    }
			}
			catch (Exception e)
			{
                model.Result = StatusResponseGenerator.GetAlertPanelWithMsgAndStat("Error.", RequestStatus.Failure);
			}
            return View(model);
		}
       
		public ActionResult View(int SiteId)
		{
			try
            {
                var provider = new SiteProvider(DomainSession.GetDataContextForUserContext(this.HttpContext));
                DeviceListViewModel model = new DeviceListViewModel(provider.GetById(SiteId));
                return View(model);
            }
			catch (Exception e)
			{
				return View();
			}
		}

        public ActionResult Index(int ServerId)
        {
            try
            {
                var provider = new DeviceProvider(DomainSession.GetDataContextForUserContext(this.HttpContext));
                DeviceViewModel model = new DeviceViewModel(provider.GetById(ServerId));
                return View(model);
            }
            catch (Exception e)
            {
                return View();
            }
        }


        // GET: /Device/Edit/<number>
        public  ActionResult Search(string key)
	    {
	        throw new NotImplementedException();
	    }
        
		public  ActionResult Edit(int Id)
		{
			try
			{
                var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
				Device dev = cont.Devices.First(d => d.Id == Id);
				DeviceEditModel model = new DeviceEditModel(dev, cont.Locations.ToList(), cont.Types.ToList() );
				return View(model);
			}
			catch (Exception e)
			{
				return View();
			}   
		 }

	    public  ActionResult Remove(int Id)
	    {
            var provider = new DeviceProvider(DomainSession.GetDataContextForUserContext(this.HttpContext));
            var Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(provider.RemoveById(Id));
            return View();
        }


	 //   public string PerformAction(int ActionId, string[] ActionParams)
		//{   
		//	try
		//	{
  //              var cont = (iotContext)System.Web.HttpContext.Current.Session["iotcontext"];
		//		DeviceAction action = cont.Actions.First(a => a.Id == ActionId);
  //              //DeviceRestfulService cl = new DeviceRestfulService();
  //              //string currValStr = action.ResultParameters.First().Value;
  //              //action.RequiredParameters.First().Value = currValStr.Equals("1") ? "0" : "1";   //toggle
  //              //action.RequiredParameters.First().Type = action.ResultParameters.First().Type;
  //              //bool result = cl.PerformDeviceAction(action);
  //              //return result == true ? "success" : "error";
  //              return "error";
  //          }
		//	catch (Exception e)
		//	{
		//		return "error";
		//	}

		//}
	}
}