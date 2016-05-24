using iotDatabaseConnector.DAL.POCO.Device.Notify;
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
     
    public class DeviceActionResult
    {


         
        [Key]
        [Required]
        public int Id { get; set; }


        private string _Value;

         
        [Required]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value != _Value)
                {
                    _Value = value;
                }
            }
        }

         
        public virtual ParameterType Type { get; set; }

         
        public string ParamDescription { get; set; }

         
        public string VisualRepresentationUrl { get; set; }

         
        [Required]
        public virtual DeviceAction Action { get; set; }

         
        public virtual List<sconnActionResultMapper> sconnMappers { get; set; }

         
        public virtual List<ActionChangeHistory> Changes { get; set; }

        public DeviceActionResult()
        {
            sconnMappers = new List<sconnActionResultMapper>();
            Changes = new List<ActionChangeHistory>();
        }
    }
   
}
