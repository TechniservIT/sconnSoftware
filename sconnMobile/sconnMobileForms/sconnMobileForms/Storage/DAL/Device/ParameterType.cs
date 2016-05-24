 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
     
    public class ParameterType
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        public string Name { get; set; }

         
        public string Description { get; set; }

         
        public string DocumentationURL { get; set; }

         
        public virtual List<DeviceParameter> Parameters { get; set; }

        public ParameterType()
        {
            Parameters = new List<DeviceParameter>();
        }
        

    }
}