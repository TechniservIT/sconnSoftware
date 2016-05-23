using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDash.DAL.Domain
{
    [DataContract(IsReference = true)]
    public class iotDomainSettings
    {

         [DataMember]
        public int MaxConcurrentUsers { get; set; }

         [DataMember]
        public int MaxSites { get; set; }

         [DataMember]
        public bool UpdateSitesOnUserLogon { get; set; }
    
    }
}