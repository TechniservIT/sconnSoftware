using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Name")]
        public string DomainName { get; set; }

        [DataMember]
        public virtual List<Site> Sites { get; set; }

        [DataMember]
        public virtual List<Location> Locations { get; set; }

        [DataMember]
        public virtual List<DeviceType> DeviceTypes { get; set; }


        [DataMember]
        public virtual List<EndpointInfo> Endpoints { get; set; }


        public iotDomain()
        {
            if (this.Locations == null)
            {
                this.Locations = new List<Location>();
            }
            if (this.DeviceTypes == null)
            {
                this.DeviceTypes = new List<DeviceType>();
            }
            if (this.Sites == null)
            {
                this.Sites = new List<Site>();
            }
            if (this.Endpoints == null)
            {
                this.Endpoints = new List<EndpointInfo>();
            }
        }

    }
}