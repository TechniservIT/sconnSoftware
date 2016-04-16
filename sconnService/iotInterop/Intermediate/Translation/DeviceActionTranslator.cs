
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Intermediate.Translation
{
    public class DeviceActionTranslator
    {
        public DeviceActionTranslator()
        {

        }

        List<DeviceAction> ActionsForDeviceType(DeviceType type)
        {
            return new List<DeviceAction>();
        }

        List<DeviceProperty> PropertiesForDeviceType(DeviceType type)
        {
            return new List<DeviceProperty>();
        }

    }
}