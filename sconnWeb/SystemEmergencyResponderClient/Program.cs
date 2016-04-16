using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using System.Management;
using System.Net;

namespace SystemEmergencyResponderClient
{
    public class EmergencyResponderClient
    {
        private MqttClient client;

        public List<string> _subscribedTopics { get; set; }
        private string[] subscribedTopics;
        private const string MQTT_BROKER_ADDRESS = "127.0.0.1";

        private void SubscribeToViolationEvent()
        {
            client.Subscribe(_subscribedTopics.ToArray(), new byte[0]);
        }
        
        public EmergencyResponderClient()
        {
            // create client instance 
            client = new MqttClient(IPAddress.Parse(MQTT_BROKER_ADDRESS));
            _subscribedTopics = new List<string>();
            _subscribedTopics.Add("/AlarmSystem/Violation");
            client.MqttMsgSubscribeReceived += Client_MqttMsgSubscribeReceived;
        }

        private void Client_MqttMsgSubscribeReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribeEventArgs e)
        {
            if (e.Topics.Contains("/Alarm/Violation"))
            {
                Shutdown_Host_System();
            }
        }

        public void Run()
        {
            string clientId = Guid.NewGuid().ToString();
            while (!client.IsConnected)
            {

                client.Connect(clientId, "OfficeServerClient", "OfficeServerPass");
                System.Threading.Thread.Sleep(2500);
            }

            SubscribeToViolationEvent();
        }


        /********** Actions on events *********/

        private void Shutdown_Host_System()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }



    }

    class Program
    {
        
        static void Main(string[] args)
        {
            EmergencyResponderClient cl = new EmergencyResponderClient();
            cl.Run();
            Console.ReadLine();
        }


    }
}
