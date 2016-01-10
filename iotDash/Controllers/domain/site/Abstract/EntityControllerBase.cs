using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace iotDash.Controllers.domain.site.Abstract
{
    public abstract class EntityControllerBase : Controller
    {
        protected IIotContextBase Icont;

        public EntityControllerBase()
        {

        }

        public EntityControllerBase(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }

        //public abstract  ActionResult Search(string key);
        //public abstract ActionResult Add();
        //public abstract ActionResult Edit(int Id);
        //public abstract ActionResult Remove(int Id);

    }
}