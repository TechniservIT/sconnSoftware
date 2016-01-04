using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.POCO.Device.Notify;
using iotDbConnector.DAL;

namespace iotDatabaseConnector.DAL.Repository.Connector.Entity
{
    public  interface IIotContextBase
    {
          event iotContextBase.DeviceUpdateEventCallbackHandler DeviceUpdateEvent;
          event iotContextBase.ParamUpdateEventCallbackHandler ParamUpdateEvent;
          event iotContextBase.ActionResultUpdateEventCallbackHandler ActionUpdateEvent;

         iotDomain IotDomain { get; set; }

         DbSet<Location> Locations { get; set; }
         DbSet<EndpointInfo> Endpoints { get; set; }
         DbSet<DeviceParameter> Parameters { get; set; }
         DbSet<ParameterType> ParamTypes { get; set; }
         DbSet<ActionParameter> ActionParameters { get; set; }
         DbSet<DeviceActionResult> ActionResultParameters { get; set; }
         DbSet<ActionChangeHistory> ActionChangeHistory { get; set; }
         DbSet<sconnActionMapper> ActionParamMappers { get; set; }
         DbSet<sconnActionResultMapper> ActionResultMappers { get; set; }
         DbSet<sconnPropertyMapper> PropertyResultMappers { get; set; }
         DbSet<DeviceProperty> Properties { get; set; }
         DbSet<DeviceAction> Actions { get; set; }
         DbSet<DeviceType> Types { get; set; }
         DbSet<DeviceCredentials> Credentials { get; set; }
         DbSet<Device> Devices { get; set; }
         DbSet<Site> Sites { get; set; }
         DbSet<iotDomain> Domains { get; set; }
         DbSet<ParameterChangeHistory> ParameterChanges { get; set; }

         void SetContextDomain(int domainId);

         int SaveChanges();
         void Fake();

    }

}
