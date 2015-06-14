using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
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
                    _Value = value;
                }
            }
        }

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
        public virtual DeviceProperty Property { get; set; }

        [DataMember]
        public virtual AIList<sconnConfigMapper> sconnMappers { get; set; }

        [DataMember]
        public virtual AIList<ParameterChangeHistory> Changes { get; set; }


    }
}