using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Device;

namespace iotDbConnector.DAL
{
    public class DeviceMapDefinition
    {
         
        [Required]
        [Key]
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public DeviceCategory Type { get; set; }
        
        [Required]
        public virtual Device Device { get; set; }


        /**********  Emap from image *********/
         
        public bool IsImageMap { get; set; }

         
        public string ImageMapUrl { get; set; }
        
         
        [Required]
        public virtual List<IoMapDefinition> IoMapDefinitions { get; set; }

        public DeviceMapDefinition()
        {
                IoMapDefinitions = new List<IoMapDefinition>();
        }
    }

}
