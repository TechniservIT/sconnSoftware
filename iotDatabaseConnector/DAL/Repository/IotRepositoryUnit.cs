using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace iotDatabaseConnector.DAL.Repository
{
    public class IotRepositoryUnit : IDisposable
    {
        private iotContextBase context;
        private iotRepository<iotDomain> domainRepository;
        private iotRepository<Site> siteRepository;
        private iotRepository<Device> deviceRepository;

        public IotRepositoryUnit(iotContextBase cont)
        {
            this.context = cont;
        }

        public iotRepository<iotDomain> DomainRepository
        {
            get
            {

                if (this.domainRepository == null)
                {
                    this.domainRepository = new iotRepository<iotDomain>(context);
                }
                return domainRepository;
            }
        }

        public iotRepository<Site> SiteRepository
        {
            get
            {

                if (this.siteRepository == null)
                {
                    this.siteRepository = new iotRepository<Site>(context);
                }
                return siteRepository;
            }
        }

        public iotRepository<Device> DeviceRepository
        {
            get
            {

                if (this.deviceRepository == null)
                {
                    this.deviceRepository = new iotRepository<Device>(context);
                }
                return deviceRepository;
            }

        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                 //   context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    
}
