
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary;
using uPLibrary.Networking.M2Mqtt;

namespace iotDomainController.DomainService.Providers
{
    public class MqttDomainProvider : IDomainProvider
    {

        public event EventHandler DeviceJoinedDomain;

        public event EventHandler DeviceDisconnected;

        public event EventHandler PropertyChanged;

        public event EventHandler DeviceUpdated;

        public event EventHandler DeviceSubscribedToEvent;

        private MqttBroker broker;


        public void OnDeviceJoinedDomain(object sender, EventArgs e)
        {

        }

        public void OnDeviceDisconnected(object sender, EventArgs e)
        {

        }
        public void OnPropertyChanged(object sender, EventArgs e)
        {

        }

        public void OnDeviceUpdated(object sender, EventArgs e)
        {

        }

        public void OnDeviceSubscribedToEvent(object sender, EventArgs e)
        {

        }

        public List<Device> GetConnectedDevices()
        {
            return new List<Device>();
        }


        public MqttDomainProvider()
        {
            broker = new MqttBroker();
            broker.ClientDisconnected += OnDeviceDisconnected;
            broker.DidAcceptNewClient += OnDeviceJoinedDomain;
            broker.DidRecievePublishMessageFromClient += OnPropertyChanged;

            broker.Start();
        }

    }
}
