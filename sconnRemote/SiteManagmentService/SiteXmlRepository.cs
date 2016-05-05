using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector;
using sconnConnector.POCO.Config;

namespace SiteManagmentService
{
    [Export(typeof(ISiteRepository))]
    public class SiteXmlRepository : ISiteRepository
    {
        private List<sconnSite> Sites;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private sconnDataSrc _configSource = new sconnDataSrc();

        public bool SyncGet { get; set; }

        [ImportingConstructor]
        public SiteXmlRepository()
        {
            Sites = new List<sconnSite>();
            Load();
        }

        public sconnSite GetSiteById(string Id)
        {
            if (SyncGet)    //reload before query
            {
                Load();
            }
            return Sites.FirstOrDefault(s => s.Id.Equals(Id));
        }

        public List<sconnSite> GetAll()
        {
            if (SyncGet)    //reload before query
            {
                Load();
            }
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
                else
                {
                    this.Add(site);
                }
            }
        }

        public void Delete(sconnSite site)
        {
            Sites.Remove(site);
            Save();
        }

        public void Add(sconnSite site)
        {
            Sites.Add(site);
            Save();
        }



        public void Save()
        {
            _configSource.SaveConfig(DataSourceType.xml);
        }

        public void Load()
        {
            _configSource.LoadConfig(DataSourceType.xml);
        }

    }
}
