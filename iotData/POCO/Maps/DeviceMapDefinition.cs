using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotDbConnector.DAL
{
    public class DeviceMapDefinition
    {
        [DataMember]
        [Required]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int DeviceId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public DeviceCategory Type { get; set; }

        [DataMember]
        [Required]
        public virtual MapDefinition Definition { get; set; }

        [DataMember]
        public virtual List<IoMapDefinition> IoMapDefinitions { get; set; }
    }

}
