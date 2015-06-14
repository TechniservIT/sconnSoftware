using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using iotDbConnector.DAL;
using iotServiceProvider;
using iotDash.Service;

namespace iotDash.RealTime.SignalR.DeviceStatusUpdater
{

    public class DeviceStatusUpdaterHub : Hub
    {

        public delegate void DeviceEventCallbackHandler(Device dev);
        public static event DeviceEventCallbackHandler DeviceCallbackEvent;

        private IDeviceEventService client;

        [CallbackBehaviorAttribute(UseSynchronizationContext = true)]
        public class DeviceEventServiceCallback : IDeviceEventCallback
        {
            public void OnDeviceUpdated(Device dev)
            {
                DeviceCallbackEvent(dev);
            }
        }

         public DeviceStatusUpdaterHub()
         {
             InstanceContext context = new InstanceContext(new DeviceEventServiceCallback());
             client = new iotServiceConnector().GetEventClient(context);
             
             DeviceEventCallbackHandler callbackHandler = new DeviceEventCallbackHandler(PublishDeviceUpdate);
             DeviceCallbackEvent += callbackHandler;

             client.Subscribe();
         }

         public void PublishDeviceUpdate(Device dev)
         {
             string jsonData = JsonConvert.SerializeObject(dev);

             //send to clients
             //Clients.Client(dev.Id.ToString()).UpdateDevice(jsonData);

             //TODO only subscribed clients

             //TODO better cast to POCO
             List<DeviceProperty> props = dev.Properties.ToList();
             foreach (var item in props)
             {
                 item.Device = null;
                 foreach (var resp  in item.ResultParameters)
                 {
                     resp.Type.Parameters = null;
                     resp.Action = null;
                     resp.Property = null;
                     resp.sconnMappers = null;
                 }
             }

             List<DeviceAction> acts = dev.Actions.ToList();
             foreach (var item in acts)
             {
                 item.Device = null;
                 foreach (var resp in item.ResultParameters)
                 {
                     resp.Type.Parameters = null;
                     resp.Action = null;
                     resp.Property = null;
                     resp.sconnMappers = null;
                 }
                 foreach (var resp in item.RequiredParameters)
                 {
                     resp.Type.Parameters = null;
                     resp.Action = null;
                     resp.Action = null;
                     resp.sconnMappers = null;
                 }
             }

             //Publish Properties & Actions
             string jsonProps = JsonConvert.SerializeObject(dev.Properties.ToList());
             string jsonActs =  JsonConvert.SerializeObject(dev.Actions.ToList());

             Clients.All.updateDevice(jsonProps);
             Clients.All.updateDevice(jsonActs);
         }

        public void UpdateProperty(string propertyData)
         {
                DeviceProperty prop = (DeviceProperty)JsonConvert.DeserializeObject(propertyData);
                IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                cl.PropertyUpdate(prop);
        }

        public void UpdateParameter(string paramData)
        {
            DeviceParameter param = (DeviceParameter)JsonConvert.DeserializeObject(paramData);
            IiotDomainService cl = new iotServiceConnector().GetDomainClient();
            cl.ResParamUpdate(param);
        }

    }
}