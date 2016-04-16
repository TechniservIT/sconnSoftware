using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Common
{
    public class EndpointInfo
    {
        [Key]
        [Required]
        public int EndpointId { get; set; }

        [Required]
        public string Hostname { get; set; }

        public int Port { get; set; }


        public bool RequiresAuthentication { get; set; }


        public bool SupportsAllJoynProtocol { get; set; }

        public bool SupportsCoAPProtocol { get; set; }

        public bool SupportsMQTTProtocol { get; set; }

        public bool SupportsRESTfulProtocol { get; set; }

        public bool SupportsSconnProtocol { get; set; }

        public bool SupportsOnvifProtocol { get; set; }

        public virtual Device Device { get; set; }

        public EndpointInfo()
        {

        }

        public EndpointInfo(    string hostname, int port)
        {
            this.Hostname = hostname;
            this.Port = port;
        }
        
    }
}