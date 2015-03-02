using iodash.Models.Common;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace iotDash.Intermediate
{
    public class iotTaskHandler
    {

        public Task<DeviceAction> PerformActionAsync(DeviceAction action)
        {
            return Task.Run(() =>
            {
                DeviceAction devparam = new DeviceAction();
                // find protocol 
                //TODO protocol priority
                EndpointInfo epi = action.Device.EndpInfo;
                if (epi.SupportsSconnProtocol)
                {

                }
                else if (epi.SupportsRESTfulProtocol)
                {

                }
                else if (epi.SupportsCoAPProtocol)
                {

                }
                else if (epi.SupportsMQTTProtocol)
                {

                }
                else if (epi.SupportsAllJoynProtocol)
                {

                }

                //perform action

               //parse result
                return devparam;
            });
        }

        public Task<DeviceProperty> ReadPropertyAsync(DeviceProperty property)
        {
            return Task.Run(() =>
            {
                DeviceProperty devparam = new DeviceProperty();
                return devparam;
            });
        }


        public Task<List<DeviceProperty>> ReadPropertiesAsync(List<DeviceProperty> property)
        {
            return Task.Run(() =>
            {
                List<DeviceProperty> devparam = new List<DeviceProperty>();
                return devparam;
            });
        }


    }

}