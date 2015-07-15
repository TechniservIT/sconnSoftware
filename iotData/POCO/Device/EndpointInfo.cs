using iotData.POCO.Surveillance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
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
        public string Hostname { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public bool RequiresAuthentication { get; set; }

        [DataMember]
        public bool SupportsAllJoynProtocol { get; set; }

        [DataMember]
        public bool SupportsCoAPProtocol { get; set; }

        [DataMember]
        public bool SupportsMQTTProtocol { get; set; }

        [DataMember]
        public bool SupportsRESTfulProtocol { get; set; }

        [DataMember]
        public bool SupportsSconnProtocol { get; set; }

        [DataMember]
        public bool SupportsOnvifProtocol { get; set; }

        [DataMember]
        public virtual List<Device> Devices { get; set; }

        [DataMember]
        public virtual List<IpCamera> Cameras { get; set; }


        [DataMember]
        public virtual iotDomain Domain { get; set; }



        public EndpointInfo()
        {
            Devices = new List<Device>();
        }


    }
}