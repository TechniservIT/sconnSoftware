using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;

namespace SiteManagmentService
{


    public interface ISiteRepository
    {
        sconnSite GetSiteById(string Id);
        List<sconnSite> GetAll();
        void Update(sconnSite site);
        void Delete(sconnSite site);
        void Add(sconnSite site);
        void Save();
        void Load();
    }

}
