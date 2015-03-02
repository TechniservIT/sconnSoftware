using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using iotDatabaseConnector.DAL.POCO.Device.Notify;

namespace iotDbConnector.DAL
{
    
    public  class  iotContext : DbContext
    {
        public iotContext()
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
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