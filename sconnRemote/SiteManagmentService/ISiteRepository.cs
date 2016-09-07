using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<sconnSite> GetAll();
        void Update(sconnSite site);
        void Delete(sconnSite site);
        void Add(sconnSite site);
        void Save();
        void Load();

        sconnSite GetCurrentSite();
        void SetCurrentSite(sconnSite site);
    }

}
