

using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using iotDash.Identity;
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

        private IotDeviceAuthorizationService AuthService;


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

        public void Start()
        {
            broker.Start();
        }

        public void Stop()
        {
            broker.Stop();
        }

        public bool HandleAuth(string u, string p)
        {
            //using (var handler = new HttpClientHandler { Credentials = ... })
            //using (var client = new HttpClient(handler))
            //{
            //    var result = await client.GetAsync(...);
            //}
            //TODO AUTH SERVICE
            return true;
        }

        public MqttDomainProvider()
        {
            broker = new MqttBroker();

           // AuthService = new IotDeviceAuthorizationService();

            broker.ClientDisconnected += OnDeviceDisconnected;
            broker.DidAcceptNewClient += OnDeviceJoinedDomain;
            broker.DidRecievePublishMessageFromClient += OnPropertyChanged;

            broker.UserAuth += HandleAuth; //AuthService.AccessWithCredentials;

        }

    }
}
