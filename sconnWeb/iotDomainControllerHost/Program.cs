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
        private static List<IDomainProvider> DomainProviders;
 
        static void Main(string[] args)
        {
            DomainProviders = new List<IDomainProvider>();

            //add all providers
            MqttDomainProvider mqttprov = new MqttDomainProvider();
            DomainProviders.Add(mqttprov);


            StartDomainProviders();
        }


        static public void StartDomainProviders()
        {
            foreach (IDomainProvider provider in DomainProviders)
            {
                  
                provider.DeviceDisconnected += prov_DeviceDisconnected;
                provider.DeviceJoinedDomain += prov_DeviceJoinedDomain;
                provider.DeviceSubscribedToEvent += prov_DeviceSubscribedToEvent;
                provider.DeviceUpdated += prov_DeviceUpdated;
                provider.PropertyChanged += prov_PropertyChanged;
                provider.Start();

            }
        }



        static void prov_PropertyChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {
                
               
            }
        }

        static void prov_DeviceUpdated(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {


            }
        }

        static void prov_DeviceSubscribedToEvent(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {


            }
        }

        static void prov_DeviceJoinedDomain(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {


            }
        }

        static void prov_DeviceDisconnected(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {


            }
        }
    }
}
