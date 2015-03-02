using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotInterop
{
    public class CommProtocol
    {
        public int ProtocolId { get; set; }

        public string ProtocolName { get; set; }

        public int ProtocolType { get; set; }

        public virtual ICollection<EndpointInfo> Endpoints { get; set; }
    }
}