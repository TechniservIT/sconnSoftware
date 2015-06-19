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
        public string Id { get; set; }

        [DataMember]
        [Required]
        public string DomainName { get; set; }

        [DataMember]
        public virtual AIList<Site> Sites { get; set; }

        [DataMember]
        public virtual AIList<Location> Locations { get; set; }

        [DataMember]
        public virtual AIList<DeviceType> DeviceTypes { get; set; }

        public iotDomain()
        {
            if (this.Locations == null)
            {
                this.Locations = new AIList<Location>();
            }
            if (this.DeviceTypes == null)
            {
                this.DeviceTypes = new AIList<DeviceType>();
            }
            if (this.Sites == null)
            {
                this.Sites = new AIList<Site>();
            }
        }

    }
}