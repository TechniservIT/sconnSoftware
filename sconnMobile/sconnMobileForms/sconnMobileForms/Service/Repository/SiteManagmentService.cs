using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;

namespace SiteManagmentService
{
    public class SiteManagmentService  : ISiteManagmentService
    {
        private List<sconnSite> Sites; 

        public sconnSite GetSiteById(string Id)
        {
            return Sites.Where(s => s.Id.Equals(Id)).FirstOrDefault();
        }

        public List<sconnSite> GetAll()
        {
            return Sites;
        }

        public void Update(sconnSite site)
        {
           
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
