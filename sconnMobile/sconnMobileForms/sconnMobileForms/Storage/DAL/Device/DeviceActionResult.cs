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
    [DataContract(IsReference = true)]
    public class DeviceActionResult
    {


        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }


        private string _Value;

        [DataMember]
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
        public virtual List<sconnActionResultMapper> sconnMappers { get; set; }

        [DataMember]
        public virtual List<ActionChangeHistory> Changes { get; set; }

        public DeviceActionResult()
        {
            sconnMappers = new List<sconnActionResultMapper>();
            Changes = new List<ActionChangeHistory>();
        }
    }
   
}
