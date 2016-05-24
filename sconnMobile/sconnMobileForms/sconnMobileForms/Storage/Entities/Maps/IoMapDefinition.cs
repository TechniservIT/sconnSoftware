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
         
        [Key]
        public int Id { get; set; }

         
	    [Required]
        public int IoId { get; set; }

         
        [Required]
        public int DeviceId { get; set; }

         
        public string Description { get; set; }

         
	    [Required]
        public DeviceIoCategory Type { get; set; }

         
        public int X { get; set; }

         
        public int Y { get; set; }

         
        public double Latitude { get; set; }

         
        public double Longitude { get; set; }

         
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
