using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using iotDash.DAL.Domain;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)] 
    public class Site
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string SiteName { get; set; }

        [DataMember]
        public virtual Location siteLocation { get; set; }

        [DataMember]
        public virtual List<Device> Devices { get; set; }

        [DataMember]
        [Required]
        public virtual iotDomain Domain { get; set; }

        public Site()
        {
            Devices = new List<Device>();

        }

        public void Load(Site other)
        {
            this.Devices = other.Devices;
            this.Domain = other.Domain;
            this.Id = other.Id;
            this.SiteName = other.SiteName;
            this.siteLocation = other.siteLocation;
           
        }

    }
}