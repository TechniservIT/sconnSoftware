using iotDatabaseConnector.DAL.Repository;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteManagmentService
{
    public class SiteProvider
    {
        private iotContext context;

        private iotDomain domain;

        public SiteProvider(iotContext cont)
        {
            this.context = cont;
        }

        public SiteProvider(iotContext cont, iotDomain domain) :this(cont)
        {
            this.domain = domain;
        }

        public SiteProvider(iotContext cont, int domainId) :this(cont)
        {
            this.domain = this.context.Domains.First(d => d.Id == domainId);
        }

        public List<Site> GetSites()
        {
            return domain.Sites.ToList();
        }




    }
}
