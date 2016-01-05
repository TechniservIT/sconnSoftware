using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iotDash.Controllers.domain.site.Abstract;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace iotDash.Controllers.domain.site.AlarmSystem.Abstract
{
    public class AlarmSystemControllerBase : Controller, IViewAbleController
    {
        protected IIotContextBase Icont;

        public AlarmSystemControllerBase()
        {
                
        }

        public AlarmSystemControllerBase(HttpContextBase contBase)
        {
            Icont = DomainSession.GetDataContextForUserContext(contBase);
        }
        
    }
}