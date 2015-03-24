using iotDash.Service;
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

        public DevicePropertyStatisticModel(int propId)
        {
            IiotDomainService cl = new iotServiceConnector().GetDomainClient();
            DeviceProperty prop = cl.DevicePropertieWithId(propId);
            if (prop != null)
            {
                Property = prop;
            }
        }

        public DevicePropertyStatisticModel()
        {

        }


    }

    public class DeviceActionStatisticModel
    {
        public DeviceAction Action { get; set; }

        public DeviceActionStatisticModel(int actId)
        {
            IiotDomainService cl = new iotServiceConnector().GetDomainClient();
            DeviceAction act = cl.DeviceActionWithId(actId);
            if (act != null)
            {
                Action = act;
            }
        }

        public DeviceActionStatisticModel()
        {

        }
    }

}