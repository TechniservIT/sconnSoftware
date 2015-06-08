using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace iotDbConnector.DAL
{

    [DataContract(IsReference = true)]
    public class iotDomain
    {

        [DataMember]
        [Key]
        [Required]
        public int DomainId { get; set; }

        [DataMember]
        [Required]
        public string DomainName { get; set; }

        [DataMember]
        public virtual List<Site> Sites { get; set; }

        [DataMember]
        public virtual List<Location> Locations { get; set; }

      

    }
}