using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPLibrary.Networking.M2Mqtt.Interop
{
    public class MqttSystemManager
    {
        
        public event EventHandler DidRecievePublishMessage;

        public event EventHandler DidAcceptNewClient;

        public event EventHandler ClientDisconnected;

        public void MqttSendMessageToDevice(string message, string deviceId)
        {

        }


        public List<MqttClient> GetConnectedClientsList()
        {
            return new List<MqttClient>();
        }

        public List<string> GetTopicList()
        {
            return new List<string>();
        }

        public void RemoveClientWithId(string Id)
        {

        }


    }
}
