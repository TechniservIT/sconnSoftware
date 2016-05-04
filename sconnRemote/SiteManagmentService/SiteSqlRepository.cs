using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;

namespace SiteManagmentService
{
    public class SiteSqlRepository : ISiteRepository
    {
        private List<sconnSite> Sites;

        public SiteSqlRepository()
        {
            Sites = new List<sconnSite>();
        }

        public sconnSite GetSiteById(string Id)
        {
            return Sites.FirstOrDefault(s => s.Id.Equals(Id));
        }

        public List<sconnSite> GetAll()
        {
            return Sites;
        }

        public void Update(sconnSite site)
        {
            if (site != null)
            {
                sconnSite existing = Sites.FirstOrDefault(s => s.Id.Equals(site.Id));
                if (existing != null)
                {
                    existing.CopyFrom(site);
                    Save();
                }
            }
        }

        public void Delete(sconnSite site)
        {
            Sites.Remove(site);
        }

        public void Add(sconnSite site)
        {
            Sites.Add(site);
        }
        
        public void Save()
        {

        }

        public void Load()
        {

        }


    }
}
