using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.POCO.Device;

namespace iotDbConnector.DAL
{
    public class MapDefinition
    {
         
        [Required]
        [Key]
        public int Id { get; set; }
        

        [Required]
        public string Url { get; set; }

         
        [Required]
        public virtual Device Device { get; set; }

         
        public virtual List<DeviceMapDefinition> DeviceMaps { get; set; }

        public MapDefinition()
        {
                DeviceMaps = new List<DeviceMapDefinition>();
        }
    }
}
