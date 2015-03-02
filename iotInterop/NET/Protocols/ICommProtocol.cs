using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotInterop
{

    public enum CommProtocolType { CommAllJoynProtocol = 1, CommCoAPProtocol, CommMQTTProtocol, CommRESTfulProtocol, CommSconnProtocol };

    public interface ICommProtocol
    {
         bool PerformAction( DeviceAction action);

         bool PerformActions( List<DeviceAction> actions);

         bool ReadProperty( DeviceProperty property);

         bool ReadProperties( List<DeviceProperty> property);


        /* Query for actions and return if there was a change */
         bool LoadDeviceActions(Device device);

         /* Query for properties and return if there was a change */
         bool LoadDeviceProperties(Device device);

         bool ProtocolDeviceQueryAble();
    }
}