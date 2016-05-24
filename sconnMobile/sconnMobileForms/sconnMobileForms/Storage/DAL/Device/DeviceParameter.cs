using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using iotDatabaseConnector.DAL.POCO.Device.Notify;
 

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceParameter 
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
                    if (value.Length > 0)
                    {
                        _Value = value;
                    }
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
        public virtual DeviceProperty Property { get; set; }

        [DataMember]
        public virtual List<sconnConfigMapper> sconnMappers { get; set; }

        [DataMember]
        public virtual List<ParameterChangeHistory> Changes { get; set; }

        public DeviceParameter()
        {
            sconnMappers = new List<sconnConfigMapper>();
            Changes = new List<ParameterChangeHistory>();
            Value = "0";    //TODO get default value for mappers
        }
    }
}