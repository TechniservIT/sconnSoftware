using iotDatabaseConnector.DAL.Repository;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace SiteManagmentService
{
    public class SiteProvider
    {
        private iotContext context;
        
        public SiteProvider(IIotContextBase cont)
        {
            this.context = cont;
        }
        

    }
}
