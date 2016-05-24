 
using iotData.POCO.Surveillance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
     
    public class Location : IFakeAbleEntity
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        [DisplayName("Name")]
        public string LocationName { get; set; }

         
        [Required]
        [DisplayName("Latitude")]
        public double Lat { get; set; }

         
        [Required]
        [DisplayName("Longitude")]
        public double Lng { get; set; }

         
        [DisplayName("Image URL")]
        public string LocationVisualRepresentationURL { get; set; }

         
        public virtual List<Device> Devices { get; set; }

         
        public virtual List<Site> Sites { get; set; }

         
        public virtual List<IpCamera> Cameras { get; set; }



         
        public virtual iotDomain Domain { get; set; }

        public void Fake()
        {
            this.LocationName = Guid.NewGuid().ToString();
            this.Lat = 0;
            this.Lng = 0;
        }
    }
}