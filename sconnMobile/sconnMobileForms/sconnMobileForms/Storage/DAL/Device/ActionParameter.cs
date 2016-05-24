using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
    
    public class ActionParameter
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        public string Value { get; set; }

         
        public virtual ParameterType Type { get; set; }

         
        public string ParamDescription { get; set; }

         
        public string VisualRepresentationUrl { get; set; }

         
        [Required]
        public virtual DeviceAction Action { get; set; }

         
        public virtual List<sconnActionMapper> sconnMappers { get; set; }


        public bool AddSconnMapper(sconnActionMapper mapper)
        {
            return false;
        }

        public List<sconnActionMapper> GetSconnMappers()
        {
            List<sconnActionMapper> mappers = new List<sconnActionMapper>();
            return mappers;
        }



    }
}

