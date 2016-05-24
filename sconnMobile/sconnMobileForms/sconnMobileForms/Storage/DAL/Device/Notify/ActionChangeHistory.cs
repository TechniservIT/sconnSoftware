using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseConnector.DAL.POCO.Device.Notify
{
     
    public class ActionChangeHistory
    {
        [Key]
        [Required]
         
        public int ParameterChangeId { get; set; }

        [Required]
         
        public virtual DeviceActionResult Property { get; set; }

        [Required]
         
        public DateTime  Date { get; set; }

        [Required]
         
        public string Value { get; set; }

        public ActionChangeHistory()
        {
            Date = DateTime.Now;
        }
    }
}
