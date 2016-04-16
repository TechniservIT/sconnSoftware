using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;

namespace iotDbConnector.DAL
{
    public class MapDefinition
    {
        [DataMember]
        [Required]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Url { get; set; }

        [DataMember]
        [Required]
        public virtual Device Device { get; set; }

        [DataMember]
        public virtual List<DeviceMapDefinition> DeviceMaps { get; set; }

        public MapDefinition()
        {
                DeviceMaps = new List<DeviceMapDefinition>();
        }
    }
}
