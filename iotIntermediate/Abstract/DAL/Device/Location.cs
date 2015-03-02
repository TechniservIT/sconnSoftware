using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Common
{
    public class Location
    {
        [Key]
        [Required]
        public int LocationId { get; set; }

        [Required]
        public string LocationName { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public double Lng { get; set; }

        public string LocationVisualRepresentationURL { get; set; }

        public virtual ICollection<Device>  Devices {get; set;}

        public virtual ICollection<Site> Sites { get; set; }
    
        
    }
}