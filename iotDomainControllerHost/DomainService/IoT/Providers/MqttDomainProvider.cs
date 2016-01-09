using iotDbConnector.DAL;
using iotServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


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


        public bool IsDeviceRegistered(Device dev)
        {
            //IiotDomainService cl = new iotServiceConnector().GetDomainClient();
            //List<Device> devices = cl.Devices().ToList();
            //return (from d in devices
            //        where 
            //        d.DeviceName.Equals( dev.DeviceName ) 
            //        &&
            //        d.EndpInfo.Hostname.Equals( dev.EndpInfo.Hostname )
            //        select d).Count() > 0;
            return false;
        }

        public Device DeviceForMqttClient(MqttClient client)
        {
            Device dev = new Device();
            dev.DeviceName = client.ClientId;
            EndpointInfo info = new EndpointInfo();
            info.Hostname = client.clientHostname;
            dev.EndpInfo = info;
            return dev;   

        }

        public void UpdateDeviceFromPublish(Device dev, MqttMsgPublish publish)
        {
            //parse publish and update properties

            try
            {
                //sconn Parser
                //IiotDomainService cl = new iotServiceConnector().GetDomainClient();
                ////EndpointInfo endp = cl.Endpoints().Where(e=> e.Hostname.Equals(dev.EndpInfo.Hostname) && e.Port == dev.EndpInfo.Port).FirstOrDefault();
                //Device stored = cl.DeviceWithEndpoint(dev.EndpInfo);
                //if (publish.Topic.Equals("Action"))
                //{
                //    foreach (var item in stored.Actions)
                //    {
                //        if (item.ResultParameters.FirstOrDefault().sconnMappers.Where(m => m.SeqNumber.ToString().Equals(publish.Message[0])) != null)
                //        {
                //            item.ResultParameters.FirstOrDefault().Value = publish.Message[1].ToString();
                //        }
                //    }
                //}
                //else if (publish.Topic.Equals("Property"))
                //{
                //    foreach (var item in stored.Properties)
                //    {
                //        if (item.ResultParameters.FirstOrDefault().sconnMappers.Where(m => m.SeqNumber.ToString().Equals(publish.Message[0])) != null)
                //        {
                //            item.ResultParameters.FirstOrDefault().Value = publish.Message[1].ToString();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {       
             
            }

        }

        public void OnDeviceJoinedDomain(object sender, MqttClientEventArgs e)
        {

        }

        public void OnDeviceDisconnected(object sender, MqttClientEventArgs e)
        {

        }
        public void OnPropertyChanged(object sender, MqttClientEventArgs e)
        {
            MqttClient cl = e.Client;
            Device dev = DeviceForMqttClient(cl);
            UpdateDeviceFromPublish(dev, e.Message);
        }

        public void OnDeviceUpdated(object sender, MqttClientEventArgs e)
        {

        }

        public void OnDeviceSubscribedToEvent(object sender, MqttClientEventArgs e)
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

            
        }

        public void Start()
        {
            broker.Start();
        }

        public void Stop()
        {
            broker.Stop();
        }

    }
}
