using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using iotDash.DAL.Domain;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
      
    public class Site : IFakeAbleEntity
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        public string SiteName { get; set; }

         
        public virtual Location siteLocation { get; set; }

         
        public virtual List<Device> Devices { get; set; }

         
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

        public void Fake()
        {
            this.SiteName = Guid.NewGuid().ToString();
        }

    }
}