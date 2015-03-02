using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iodash.Models.Common
{

    public enum CommProtocolType { CommAllJoynProtocol = 1, CommCoAPProtocol, CommMQTTProtocol, CommRESTfulProtocol, CommSconnProtocol };

    public interface ICommProtocol
    {
         bool PerformActionAsync( DeviceAction action);

         bool PerformActionsAsync( List<DeviceAction> actions);

         bool ReadPropertyAsync( DeviceProperty property);

         bool ReadPropertiesAsync( List<DeviceProperty> property);


        /* Query for actions and return if there was a change */
         bool LoadDeviceActions(Device device);

         /* Query for properties and return if there was a change */
         bool LoadDeviceProperties(Device device);

         bool ProtocolDeviceQueryAble();
    }
}