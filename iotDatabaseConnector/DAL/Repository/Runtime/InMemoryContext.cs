using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseConnector.Runtime
{
    static  class InMemoryContext
    {
        static private iotContext cont = new iotContext();
       
        static public iotDomain GetDomainForName(string name)
        {
            return cont.Domains.First(d => d.DomainName.Equals(name));
        }

        static public bool AddDomain(iotDomain domain)
        {
            try
            {
                cont.Domains.Add(domain);
                cont.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        static public bool UpdateDomain(iotDomain domain)
        {
            try
            {
                iotDomain existing = cont.Domains.First(d=>d.Id.Equals(domain.Id));
                existing = domain;
                cont.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public bool RemoveDomain(iotDomain domain)
        {
            try
            {
                iotDomain existing = cont.Domains.First(d => d.Id.Equals(domain.Id));
                cont.Domains.Remove(existing);
                cont.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



    }

}
