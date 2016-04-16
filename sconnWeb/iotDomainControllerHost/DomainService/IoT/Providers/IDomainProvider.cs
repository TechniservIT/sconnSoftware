using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDomainController.DomainService.Providers
{
    public interface IDomainProvider
    {
        event EventHandler DeviceJoinedDomain;

        event EventHandler DeviceDisconnected;

        event EventHandler PropertyChanged;

        event EventHandler DeviceUpdated;

        event EventHandler DeviceSubscribedToEvent;


        List<Device> GetConnectedDevices();

        void Start();

        void Stop();
    
    }


}
