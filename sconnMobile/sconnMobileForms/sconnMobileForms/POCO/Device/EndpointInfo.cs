using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace sconnConnector.POCO.Device
{
     
    public class EndpointInfo 
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        [DisplayName("Hostname")]
        public string Hostname { get; set; }

         
        [DisplayName("Port")]
        public int Port { get; set; }

         
        [DisplayName("Requires Authentication")]
        public bool RequiresAuthentication { get; set; }

         
        [DisplayName("AllJoyn support")]
        public bool SupportsAllJoynProtocol { get; set; }

         
        [DisplayName("CoAP support")]
        public bool SupportsCoAPProtocol { get; set; }

         
        [DisplayName("MQTT support")]
        public bool SupportsMQTTProtocol { get; set; }

         
        [DisplayName("RESTful support")]
        public bool SupportsRESTfulProtocol { get; set; }

         
        [DisplayName("Sconn support")]
        public bool SupportsSconnProtocol { get; set; }

         
        [DisplayName("Onvif support")]
        public bool SupportsOnvifProtocol { get; set; }

         
        public virtual List<Device> Devices { get; set; }

        public EndpointInfo()
        {
            Devices = new List<Device>();
        }


        public void Fake()
        {
           this.Hostname =  Guid.NewGuid().ToString();
        }
    }
}