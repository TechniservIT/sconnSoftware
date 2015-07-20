using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
   [DataContract(IsReference = true)]
    public class ActionParameter
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Value { get; set; }

        [DataMember]
        public virtual ParameterType Type { get; set; }

        [DataMember]
        public string ParamDescription { get; set; }

        [DataMember]
        public string VisualRepresentationUrl { get; set; }

        [DataMember]
        [Required]
        public virtual DeviceAction Action { get; set; }

        [DataMember]
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

