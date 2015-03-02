using iotDomainControllerHost.iotDbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDomainController.DomainService.Providers
{
    public class AllJoynDomainProvider
    {
        event EventHandler DeviceJoinedDomain;

        event EventHandler DeviceDisconnected;

        event EventHandler PropertyChanged;

        event EventHandler DeviceUpdated;

        event EventHandler DeviceSubscribedToEvent;


        public List<Device> GetConnectedDevices()
        {
            return new List<Device>();
        }

    }
}
