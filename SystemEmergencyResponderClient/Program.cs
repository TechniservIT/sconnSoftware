using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace SystemEmergencyResponderClient
{
    public class EmergencyResponderClient
    {
        private MqttClient client;

        private void SubscribeToViolationEvent()
        {

        }

        public EmergencyResponderClient()
        {
            client = new MqttClient("");
        }

        public void Run()
        {
            SubscribeToViolationEvent();
            //timer
        }

    }

    class Program
    {




        static void Main(string[] args)
        {
            EmergencyResponderClient cl = new EmergencyResponderClient();
        }


    }
}
