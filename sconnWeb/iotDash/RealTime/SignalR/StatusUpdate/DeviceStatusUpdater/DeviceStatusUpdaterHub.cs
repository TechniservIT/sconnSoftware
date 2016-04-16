﻿using System.Collections.Generic;
using System.Linq;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace iotDash.RealTime.SignalR.StatusUpdate.DeviceStatusUpdater
{

    public class DeviceStatusUpdaterHub : Hub
    {
        private IIotContextBase cont;

        public delegate void DeviceEventCallbackHandler(Device dev);
        public event DeviceEventCallbackHandler DeviceCallbackEvent;
        
         public DeviceStatusUpdaterHub()
         {
            //cont.DeviceUpdateEvent += iotContext_DeviceUpdateEvent;
            //cont.ParamUpdateEvent += iotContext_ParamUpdateEvent;
            //cont.ActionUpdateEvent += iotContext_ActionUpdateEvent;
            // client = new DeviceRestfulService();

            // DeviceEventCallbackHandler callbackHandler = new DeviceEventCallbackHandler(PublishDeviceUpdate);
            // DeviceCallbackEvent += callbackHandler;

            //TODO Restful subcribe
            // client.Subscribe();
         }

        public DeviceStatusUpdaterHub(IIotContextBase cont) :this()
        {
            this.cont = cont;
        }

         void iotContext_ActionUpdateEvent(DeviceActionResult param)
         {
             PublishActionUpdate(param);
         }

         void iotContext_ParamUpdateEvent(DeviceParameter param)
         {
             PublishParamUpdate(param);
         }

         void iotContext_DeviceUpdateEvent(Device dev)
         {
             if (DeviceCallbackEvent != null) DeviceCallbackEvent(dev);
         }


        public void PublishActionUpdate(DeviceActionResult param)
         {
             IIotContextBase cont = new iotContext();
             //cont.Configuration.ProxyCreationEnabled = false;
             //cont.Configuration.LazyLoadingEnabled = false;

             DeviceActionResult toUpdate = cont.ActionResultParameters.Include("Action").FirstOrDefault(e => e.Id == param.Id);

             //unbind after parent
             if (toUpdate.Action != null)
             {
                 toUpdate.Action.Device = null;
                 toUpdate.Action.RequiredParameters = null;
                 toUpdate.Action.ResultParameters = null;
             }
            


             string jsonParam = JsonConvert.SerializeObject(toUpdate);
             Clients.All.updateParam(jsonParam);

         }


        public void PublishParamUpdate(DeviceParameter param)
         {
             IIotContextBase cont = new iotContext();

             DeviceParameter toUpdate = cont.Parameters.Include("Property").FirstOrDefault(e => e.Id == param.Id);
            
            //unbind after parent
              if (toUpdate.Property != null)
             {
                 toUpdate.Property.Device = null;
                 toUpdate.Property.ResultParameters = null;
             }


             string jsonParam = JsonConvert.SerializeObject(toUpdate);
             Clients.All.updateParam(jsonParam);     
 
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

        public void UpdateProperty(string propertyData, int domainId)
         {
                DeviceProperty prop = (DeviceProperty)JsonConvert.DeserializeObject(propertyData);
                //DeviceRestfulService cl = new DeviceRestfulService();
                //cl.PropertyUpdate(prop);
        }

        public void UpdateParameter(string paramData)
        {
            DeviceParameter param = (DeviceParameter)JsonConvert.DeserializeObject(paramData);
            //DeviceRestfulService cl = new DeviceRestfulService();
            //cl.ResParamUpdate(param);
        }

    }
}