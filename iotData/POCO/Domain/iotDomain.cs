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
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string DomainName { get; set; }

        [DataMember]
        public virtual AIList<Site> Sites { get; set; }

        [DataMember]
        public virtual AIList<Location> Locations { get; set; }

        [DataMember]
        public virtual AIList<DeviceType> DeviceTypes { get; set; }



    }
}