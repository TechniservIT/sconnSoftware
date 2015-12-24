using iotDatabaseConnector.DAL.Repository;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace SiteManagmentService
{
    public class SiteProvider
    {
        private IIotContextBase context;
        
        public SiteProvider(IIotContextBase cont)
        {
            this.context = cont;
        }
        
        public List<Site> GetSites()
        {
            return context.IotDomain.Sites.ToList();
        }

        public bool RemoveDevice(int id)
        {
            try
            {
                Device dev = context.Devices.First(s => s.Id == id);
                if (dev != null)
                {
                    context.Devices.Remove(dev);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
}
