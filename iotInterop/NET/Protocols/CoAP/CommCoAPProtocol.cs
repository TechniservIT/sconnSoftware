using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoAP;
using iotDbConnector.DAL;
using iotInterop.iotDbService;

namespace iotInterop
{
   

    public class CommCoAPProtocol : ICommProtocol
    {

        private void SendRequest()
        {


        }

        public bool PerformAction(DeviceAction action)
        {
            return false;
        }

        public bool PerformActions(List<DeviceAction> actions)
        {
            return false;
        }

        public bool ReadProperty(DeviceProperty property)
        {

            // new a GET request
            Request request = new Request(Method.GET);
            request.URI = new Uri("coap://"+property.Device.EndpInfo.Hostname.ToString()+"/"+property.PropertyName);
            request.Send();

            // wait for one response
            Response response = request.WaitForResponse();


            return false;
        }

        public bool ReadProperties(List<DeviceProperty> property)
        {
            return false;
        }


        /* Query for actions and return if there was a change */
        public bool LoadDeviceActions(Device device)
        {
            return false;
        }

        /* Query for properties and return if there was a change */
        public bool LoadDeviceProperties(Device device)
        {
            return false;
        }

        public bool ProtocolDeviceQueryAble()
        {
            return false;
        }

    }
}