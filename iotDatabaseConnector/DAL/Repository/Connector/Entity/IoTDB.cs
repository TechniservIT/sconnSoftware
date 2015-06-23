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

namespace iotDbConnector.DAL
{
    
    public  class  iotContext : DbContext
    {
        public iotContext() :base("iotDbConn")
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", "H:\\Inf\\PZPP\\IoT\\iotDash\\DBO");

            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
            
        }

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
                                //var mdw = ctx.MetadataWorkspace;
                                //var items = mdw.GetItems<EntityType>(DataSpace.CSpace);
                                //EntityType devparamType = items.Where(i => i.Name.Equals("DeviceParameter")).FirstOrDefault();

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

                                }

                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }


        public DbSet<Location> Locations { get; set; }

        public DbSet<EndpointInfo> Endpoints { get; set; }

        public DbSet<DeviceParameter> Parameters { get; set; }

        public DbSet<ParameterType> ParamTypes { get; set; }

        public DbSet<ActionParameter> ActionParameters { get; set; }

        public DbSet<DeviceProperty> Properties { get; set; }

        public DbSet<DeviceAction> Actions { get; set; }

        public DbSet<DeviceType> Types { get; set; }

        public DbSet<DeviceCredentials> Credentials { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<iotDomain> Domains { get; set; }

        //public DbSet<UserPermission> Permissions { get; set; }

        public DbSet<sconnConfigMapper> SconnMappers { get; set; }

        public DbSet<ParameterChangeHistory> ParameterChanges { get; set; }

    }

}