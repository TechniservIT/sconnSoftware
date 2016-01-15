using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using iotDatabaseConnector.DAL.POCO.Device.Notify;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace iotDbConnector.DAL
{
    public abstract class iotContextBase : DbContext, IIotContextBase
    {
        public iotContextBase() :base("iotDbConn")
        {
            
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }



        public void SetContextDomain(int domainId)
        {
            this.IotDomain = Queryable.First<iotDomain>(this.Domains, d => d.Id == domainId);
        }

        public iotContextBase(int DomainId) :this()
        {
            this.IotDomain = Queryable.First<iotDomain>(this.Domains, d=>d.Id == DomainId);
        }

        public iotContextBase(iotDomain domain) :this()
        {
            this.IotDomain = domain;
        }

        public void Fake()
        {
            
        }

        public delegate void DeviceUpdateEventCallbackHandler(Device dev);

        public virtual  event DeviceUpdateEventCallbackHandler DeviceUpdateEvent;

        public delegate void ParamUpdateEventCallbackHandler(DeviceParameter param);

        public virtual  event ParamUpdateEventCallbackHandler ParamUpdateEvent;

        public delegate void ActionResultUpdateEventCallbackHandler(DeviceActionResult param);

        public virtual  event ActionResultUpdateEventCallbackHandler ActionUpdateEvent;
        public iotDomain IotDomain { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<EndpointInfo> Endpoints { get; set; }
        public DbSet<DeviceParameter> Parameters { get; set; }
        public DbSet<ParameterType> ParamTypes { get; set; }
        public DbSet<ActionParameter> ActionParameters { get; set; }
        public DbSet<DeviceActionResult> ActionResultParameters { get; set; }
        public DbSet<ActionChangeHistory> ActionChangeHistory { get; set; }
        public DbSet<sconnActionMapper> ActionParamMappers { get; set; }
        public DbSet<sconnActionResultMapper> ActionResultMappers { get; set; }
        public DbSet<sconnPropertyMapper> PropertyResultMappers { get; set; }
        public DbSet<DeviceProperty> Properties { get; set; }
        public DbSet<DeviceAction> Actions { get; set; }
        public DbSet<DeviceType> Types { get; set; }
        public DbSet<DeviceCredentials> Credentials { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<iotDomain> Domains { get; set; }
        public DbSet<ParameterChangeHistory> ParameterChanges { get; set; }

        public DbSet<MapDefinition> MapDefinitions { get; set; }
        public DbSet<IoMapDefinition> IoMapDefinitions { get; set; }
        public DbSet<DeviceMapDefinition> DeviceMapDefinitions { get; set; }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges(); // Important!

            ObjectContext ctx = ((IObjectContextAdapter)this).ObjectContext;

            List<ObjectStateEntry> objectStateEntryList =
                ctx.ObjectStateManager.GetObjectStateEntries(EntityState.Added
                                                             | EntityState.Modified
                                                             | EntityState.Deleted)
                    .ToList();

            foreach (ObjectStateEntry entry in objectStateEntryList)
            {
                if (!entry.IsRelationship)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // write log...
                            break;
                        case EntityState.Deleted:
                            // write log...
                            break;
                        case EntityState.Modified:
                        {
                            if (ObjectContext.GetObjectType(entry.Entity.GetType()) == typeof(DeviceParameter))  
                            {

                                DbDataRecord original = entry.OriginalValues;
                                string oldValue = original.GetValue(
                                    original.GetOrdinal("Value"))
                                    .ToString();

                                CurrentValueRecord current = entry.CurrentValues;
                                string newValue = current.GetValue(
                                    current.GetOrdinal("Value"))
                                    .ToString();

                                if (oldValue != newValue) // probably not necessary
                                {
                                    ParameterChangeHistory hist = new ParameterChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = (DeviceParameter)(object)entry.Entity;
                                    hist.Value = newValue;
                                    this.ParameterChanges.Add(hist);
                                }

                                if (this.ParamUpdateEvent != null)
                                {
                                    Task.Factory.StartNew(() => this.ParamUpdateEvent((DeviceParameter)entry.Entity));
                                }
                                    
                                    

                                
                            }
                            else if (ObjectContext.GetObjectType(entry.Entity.GetType()) == typeof(DeviceActionResult))
                            {

                                DbDataRecord original = entry.OriginalValues;
                                string oldValue = original.GetValue(
                                    original.GetOrdinal("Value"))
                                    .ToString();

                                CurrentValueRecord current = entry.CurrentValues;
                                string newValue = current.GetValue(
                                    current.GetOrdinal("Value"))
                                    .ToString();

                                if (oldValue != newValue) // probably not necessary
                                {
                                    ActionChangeHistory hist = new ActionChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = (DeviceActionResult)(object)entry.Entity;
                                    hist.Value = newValue;
                                    this.ActionChangeHistory.Add(hist);
                                }

                                if (this.ActionUpdateEvent != null)
                                {
                                    Task.Factory.StartNew(() => this.ActionUpdateEvent((DeviceActionResult)entry.Entity));
                                }
                                    


                            }
                            else if (ObjectContext.GetObjectType(entry.Entity.GetType()) == typeof(Device))
                            {
                                Task.Factory.StartNew(() => this.DeviceUpdateEvent((Device)entry.Entity));
                            }
                            break;
                        }
                    }
                }
            }
            return base.SaveChanges();
        }
    }

    public  class  iotContext : iotContextBase
    {
        public override  event DeviceUpdateEventCallbackHandler DeviceUpdateEvent;

        public override  event ParamUpdateEventCallbackHandler ParamUpdateEvent = delegate { };

        public override  event ActionResultUpdateEventCallbackHandler ActionUpdateEvent = delegate { };

        public iotContext() : base()
        {
        }

        public iotContext(int DomainId) : base(DomainId)
        {
        }

        public iotContext(iotDomain domain) : base(domain)
        {
        }

        public void Fake()
        {
            Location loc = new Location();
            loc.Lat = 1.11;
            loc.Lng=22.11;
            loc.LocationName = Guid.NewGuid().ToString();
            this.Locations.Add(loc);
            this.SaveChanges();
            Location storedloc = this.Locations.FirstOrDefault(nl => nl.LocationName == loc.LocationName);
            iotDomain d = new iotDomain();
            d.DomainName = Guid.NewGuid().ToString();
            d.Id = 0;
            d.Sites = new List<Site>();
            this.Domains.Add(d);
            this.SaveChanges();

            iotDomain stored = this.Domains.First(s => s.DomainName == d.DomainName);
            Site site = new Site();
            site.Id = 0;
            site.Devices = new List<Device>();
            site.Domain = stored;
            site.SiteName = Guid.NewGuid().ToString();
            this.Sites.Add(site);
            this.SaveChanges();

        }

    }

}