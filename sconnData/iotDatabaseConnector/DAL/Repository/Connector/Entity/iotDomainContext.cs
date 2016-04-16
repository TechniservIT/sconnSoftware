using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseConnector.DAL.Repository.Connector.Entity
{
    public class iotDomainContext : iotContext
    {
        private iotDomain Domain;

        public iotDomainContext(iotDomain domain)
        {
            this.Domain = domain;
        }

        public iotDomainContext(int DomainId)
        {

        }
    }


}
