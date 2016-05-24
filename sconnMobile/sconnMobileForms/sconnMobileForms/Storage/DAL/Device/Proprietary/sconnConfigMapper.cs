using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
 
namespace iotDbConnector.DAL
{

    public interface sconnConfigMapper
    {
        [Key]
        [Required]
         int MapperId { get; set; }

         
        [Required]
         int ConfigType { get; set; }


         
        [Required]
         int SeqNumber { get; set; }



    }
}