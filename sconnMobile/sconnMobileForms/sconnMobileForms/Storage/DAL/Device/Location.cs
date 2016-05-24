 
using iotData.POCO.Surveillance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class Location : IFakeAbleEntity
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Name")]
        public string LocationName { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Latitude")]
        public double Lat { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Longitude")]
        public double Lng { get; set; }

        [DataMember]
        [DisplayName("Image URL")]
        public string LocationVisualRepresentationURL { get; set; }

        [DataMember]
        public virtual List<Device> Devices { get; set; }

        [DataMember]
        public virtual List<Site> Sites { get; set; }

        [DataMember]
        public virtual List<IpCamera> Cameras { get; set; }



        [DataMember]
        public virtual iotDomain Domain { get; set; }

        public void Fake()
        {
            this.LocationName = Guid.NewGuid().ToString();
            this.Lat = 0;
            this.Lng = 0;
        }
    }
}