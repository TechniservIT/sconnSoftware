 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class ParameterType
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string DocumentationURL { get; set; }

        [DataMember]
        public virtual AIList<DeviceParameter> Parameters { get; set; }

        

    }
}