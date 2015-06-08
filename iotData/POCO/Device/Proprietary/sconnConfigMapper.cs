using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{

    [DataContract(IsReference = true)]
    public class sconnConfigMapper
    {
        [Key]
        [Required]
        [DataMember]
        public int MapperId { get; set; }

        [DataMember]
        [Required]
        public int ConfigType { get; set; }


        [DataMember]
        [Required]
        public int SeqNumber { get; set; }

        [DataMember]
        public virtual DeviceParameter Parameter { get; set; }

        [DataMember]
        public virtual ActionParameter ActionParam { get; set; }



    }
}