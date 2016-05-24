using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotDbConnector.DAL
{
    
     
    public class sconnPropertyMapper : sconnConfigMapper
    {
        [Key]
        [Required]
         
        public int MapperId { get; set; }

         
        [Required]
        public int ConfigType { get; set; }


         
        [Required]
        public int SeqNumber { get; set; }

         
        public virtual DeviceParameter Parameter { get; set; }




    }
}
