using iotDomainController.DomainService.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttBrokerRuntime
{

    /********* Debug & test service operation *********/
    class Program
    {
        static void Main(string[] args)
        {
            MqttDomainProvider prov = new MqttDomainProvider();
            prov.DeviceDisconnected += prov_DeviceDisconnected;
            prov.DeviceJoinedDomain += prov_DeviceJoinedDomain;
            prov.DeviceSubscribedToEvent += prov_DeviceSubscribedToEvent;
            prov.DeviceUpdated += prov_DeviceUpdated;
            prov.PropertyChanged += prov_PropertyChanged;

            prov.Start();

        }

        static void prov_PropertyChanged(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        static void prov_DeviceUpdated(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        static void prov_DeviceSubscribedToEvent(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        static void prov_DeviceJoinedDomain(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        static void prov_DeviceDisconnected(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }
    }
}
