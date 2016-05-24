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
        [Required]
        public int DeviceId { get; set; }

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
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        [Required]
        public virtual DeviceMapDefinition Definition { get; set; }

        public IoMapDefinition()
        {

        }
        

        public void Copy(IoMapDefinition other)
        {
            this.Description = other.Description;
            this.DeviceId = other.DeviceId;
            this.IoId = other.IoId;
            this.Latitude = other.Latitude;
            this.Longitude = other.Longitude;
            this.Type = other.Type;
            this.X = other.X;
            this.Y = other.Y;
        }
    }

}
