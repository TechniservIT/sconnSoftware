
using iotDbConnector.DAL;
using iotServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Models
{
    public class DevicePropertyStatisticModel
    {
        public DeviceProperty Property { get; set; }

        public DevicePropertyStatisticModel(DeviceProperty prop)
        {
            //DeviceRestfulService cl = new DeviceRestfulService();
            //DeviceProperty prop = cl.DevicePropertieWithId(propId,);
            //if (prop != null)
            //{
            //    Property = prop;
            //}

            this.Property = prop;
        }

        public DevicePropertyStatisticModel()
        {

        }


    }

    public class DeviceActionStatisticModel
    {
        public DeviceAction DeviceAction { get; set; }

        public DeviceActionStatisticModel(DeviceAction action)
        {
            //DeviceRestfulService cl = new DeviceRestfulService();
            //DeviceAction act = cl.DeviceActionWithId(actId);
            //if (act != null)
            //{
            //    Action = act;
            //}

            DeviceAction = action;
        }

        public DeviceActionStatisticModel()
        {

        }
    }

}