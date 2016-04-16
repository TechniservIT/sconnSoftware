using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{

    public interface sconnConfigMapper
    {
        [Key]
        [Required]
        [DataMember]
         int MapperId { get; set; }

        [DataMember]
        [Required]
         int ConfigType { get; set; }


        [DataMember]
        [Required]
         int SeqNumber { get; set; }



    }
}