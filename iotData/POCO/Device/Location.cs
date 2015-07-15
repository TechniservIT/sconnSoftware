 
using iotData.POCO.Surveillance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class Location
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string LocationName { get; set; }

        [DataMember]
        [Required]
        public double Lat { get; set; }

        [DataMember]
        [Required]
        public double Lng { get; set; }

        [DataMember]
        public string LocationVisualRepresentationURL { get; set; }

        [DataMember]
        public virtual List<Device> Devices { get; set; }

        [DataMember]
        public virtual List<Site> Sites { get; set; }

        [DataMember]
        public virtual List<IpCamera> Cameras { get; set; }



        [DataMember]
        public virtual iotDomain Domain { get; set; }
        
    }
}