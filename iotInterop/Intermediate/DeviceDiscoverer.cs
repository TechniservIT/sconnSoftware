using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iotInterop.iotDbService;

namespace iotInterop
{
    public static class DeviceDiscoverer
    {

        public static Boolean QueryDeviceActionsWithProtocol(Device device, ICommProtocol protocol)
        {
            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceActions(device);
            }
            return false;
        }
        public static Boolean QueryDevicePropertiesWithProtocol(Device device, ICommProtocol protocol)
        {
            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceProperties(device);
            }
            return false;
        }


    }


}