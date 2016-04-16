using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Common
{
    public class DeviceType
    {
        [Key]
        [Required]
        public int DeviceTypeId { get; set; }

        [Required]
        public string TypeName { get; set; }

        public string TypeDescription { get; set; }

        public string VisualRepresentationURL { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    
        
    }
}