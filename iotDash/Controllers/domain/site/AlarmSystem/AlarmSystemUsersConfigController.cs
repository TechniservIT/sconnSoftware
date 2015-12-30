using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService;
using iotDash.Content.Dynamic.Status;
using iotDash.Models;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract.Auth;

namespace iotDash.Controllers.domain.site.AlarmSystem
{
    public class AlarmSystemUsersConfigController : Controller
    {
        private IIotContextBase Icont;
        private UsersConfigurationService _provider;


        public AlarmSystemUsersConfigController(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }

      

        public ActionResult Add()
        {
            AlarmSystemUserAddModel model = new AlarmSystemUserAddModel();
            return View(model);
        }

        public ActionResult Edit(sconnUser User)
        {
            AlarmSystemUserAddModel model = new AlarmSystemUserAddModel(User);
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            AlarmSystemUserAddModel model = new AlarmSystemUserAddModel();
            this._provider = new UsersConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            model.User = _provider.GetById(Id);
            return View(model);
        }

        public ActionResult Search(string key)
        {
            this._provider = new UsersConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemUserConfigModel model = new AlarmSystemUserConfigModel(_provider.GetAll());
            if (!String.IsNullOrEmpty(key))
            {
                model.Users = model.Users.Where(d => d.Login.Contains(key) ).ToList();
            }
            return View(model);
        }

        public ActionResult Remove(int Id)
        {
            this._provider = new UsersConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            AlarmSystemUserConfigModel model = new AlarmSystemUserConfigModel(_provider.GetAll());
            bool remRes = _provider.RemoveById(Id);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }

        public ActionResult Remove(sconnUser Rcpt)
        {
            AlarmSystemUserAddModel model = new AlarmSystemUserAddModel();
            this._provider = new UsersConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
            var remRes = this._provider.Remove(Rcpt);
            model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(remRes);
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Add(AlarmSystemUserAddModel model)
        {
            try
            {
                this._provider = new UsersConfigurationService(DomainSession.GetAlarmConfigForContextSession(this.HttpContext));
                if (ModelState.IsValid)
                {
                    var res = (_provider.Add(model.User));
                    model.Result = StatusResponseGenerator.GetStatusResponseResultForReturnParam(res);
                }
            }
            catch (Exception e)
            {
                model.Result = StatusResponseGenerator.GetRequestResponseCriticalError();
            }
            return View(model);
        }

        public ActionResult View(int DeviceId)
        {
            this._provider = new UsersConfigurationService(DomainSession.GetAlarmConfigForContextWithDeviceId(this.HttpContext, DeviceId));
            AlarmSystemUserConfigModel model = new AlarmSystemUserConfigModel(this._provider.GetAll());
            return View(model);
        }

    }
}