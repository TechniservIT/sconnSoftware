using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotDbConnector.DAL
{
    public class IoMapDefinition
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
	    [Required]
        public int IoId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
	    [Required]
        public DeviceIoCategory Type { get; set; }

        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }

        [DataMember]
	    [Required]
        public double Latitude { get; set; }

        [DataMember]
	    [Required]
        public double Longitude { get; set; }

        [DataMember]
	    [Required]
        public virtual DeviceMapDefinition Definition { get; set; }
     

    }

}
