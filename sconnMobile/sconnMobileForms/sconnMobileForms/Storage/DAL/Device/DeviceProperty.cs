 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
     
    public class DeviceProperty
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        public string PropertyName { get; set; }

         
        public string  PropertyDescription { get; set; }

         
        public string VisualRepresentationURL { get; set; }

         
        public virtual List<DeviceParameter> ResultParameters { get; set; }

         
        public DateTime LastUpdateTime { get; set; }

         
        [Required]
        public virtual Device Device { get; set; }

        public DeviceProperty()
        {
            ResultParameters = new List<DeviceParameter>();
            LastUpdateTime = DateTime.Now;
        }

    }


}