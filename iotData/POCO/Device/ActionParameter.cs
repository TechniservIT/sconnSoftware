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
        [Required]
        public virtual ParameterType Type { get; set; }

        [DataMember]
        public string ParamDescription { get; set; }

        [DataMember]
        public string VisualRepresentationUrl { get; set; }

        [DataMember]
        public virtual DeviceAction Action { get; set; }

        [DataMember]
        public virtual AIList<sconnConfigMapper> sconnMappers { get; set; }


        public bool AddSconnMapper(sconnConfigMapper mapper)
        {
            return false;
        }

        public List<sconnConfigMapper> GetSconnMappers()
        {
            List<sconnConfigMapper> mappers = new List<sconnConfigMapper>();
            return mappers;
        }



    }
}

