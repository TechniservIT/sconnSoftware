using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IotServiceAbstract;
using sconnConnector.POCO.Config;

namespace SiteListService
{

    public interface ISiteManagmentService : IEntityService<sconnSite>
    {

    }

    public class SconnSiteServiceProvider : ISiteManagmentService
    {
        public bool Add(sconnSite entity)
        {
            throw new NotImplementedException();
        }

        public bool Remove(sconnSite entity)
        {
            throw new NotImplementedException();
        }

        public List<sconnSite> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(sconnSite entity)
        {
            throw new NotImplementedException();
        }

        public sconnSite GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveById(int Id)
        {
            throw new NotImplementedException();
        }
    }

}
