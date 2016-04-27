using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace sconnConnector.POCO.Device
{
    [DataContract(IsReference = true)]
    public class EndpointInfo 
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Hostname")]
        public string Hostname { get; set; }

        [DataMember]
        [DisplayName("Port")]
        public int Port { get; set; }

        [DataMember]
        [DisplayName("Requires Authentication")]
        public bool RequiresAuthentication { get; set; }

        [DataMember]
        [DisplayName("AllJoyn support")]
        public bool SupportsAllJoynProtocol { get; set; }

        [DataMember]
        [DisplayName("CoAP support")]
        public bool SupportsCoAPProtocol { get; set; }

        [DataMember]
        [DisplayName("MQTT support")]
        public bool SupportsMQTTProtocol { get; set; }

        [DataMember]
        [DisplayName("RESTful support")]
        public bool SupportsRESTfulProtocol { get; set; }

        [DataMember]
        [DisplayName("Sconn support")]
        public bool SupportsSconnProtocol { get; set; }

        [DataMember]
        [DisplayName("Onvif support")]
        public bool SupportsOnvifProtocol { get; set; }

        [DataMember]
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