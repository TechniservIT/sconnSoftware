using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using SQLite;
using NLog;

namespace SiteManagmentService
{
    public class SiteSqlRepository : ISiteRepository
    {
       // private ObservableCollection<sconnSite> Sites;
        private SQLiteConnection _connection;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public string DatabasePath { get; set; }


        private void Fake()
        {
            sconnSite site1 = new sconnSite();
            site1.authPasswd = Guid.NewGuid().ToString();
            site1.serverIP = Guid.NewGuid().ToString();
            this.Add(site1);
        }

        public void Database_Init()
        {
            try
            {
                DatabasePath = Path.Combine(  Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sconnSites.db3");
                _connection = new SQLiteConnection(DatabasePath);
                _connection.CreateTable<sconnSite>(CreateFlags.AutoIncPK);
                var table = _connection.Table<sconnSite>();

                Fake();
                // Sites = new ObservableCollection<sconnSite>(table.ToList());

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
        

        public SiteSqlRepository() 
        {
           // Sites = new ObservableCollection<sconnSite>();
            Database_Init();
        }

        public sconnSite GetSiteById(string Id)
        {
            try
            {
                var table = _connection.Table<sconnSite>();
                var sites = new ObservableCollection<sconnSite>(table.ToList());
                return sites.FirstOrDefault(s => s.Id.Equals(Id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new sconnSite();
            }
        }

        public ObservableCollection<sconnSite> GetAll()
        {
            try
            {
                var table = _connection.Table<sconnSite>();
                var sites = new ObservableCollection<sconnSite>(table.ToList());
                return sites;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new ObservableCollection<sconnSite>();
            }
        }

        public void Update(sconnSite site)
        {
            try
            {
                if (site != null)
                {
                    var table = _connection.Table<sconnSite>();
                    var sites = new ObservableCollection<sconnSite>(table.ToList());
                    sconnSite existing = sites.FirstOrDefault(s => s.Id.Equals(site.Id));
                    if (existing != null)
                    {
                        existing.CopyFrom(site);
                        Save();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }

        }

        public void Delete(sconnSite site)
        {
            try
            {
                var rowcount = _connection.Delete<sconnSite>(site.Id); // Id is the primary key
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }

        }

        public void Add(sconnSite site)
        {
            try
            {
                _connection.Insert(site);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
        

        public void Save()
        {

        }

        public void Load()
        {

        }


    }
}
