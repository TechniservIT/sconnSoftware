 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceProperty
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string PropertyName { get; set; }

        [DataMember]
        public string  PropertyDescription { get; set; }

        [DataMember]
        public string VisualRepresentationURL { get; set; }

        [DataMember]
        public virtual List<DeviceParameter> ResultParameters { get; set; }

        [DataMember]
        public DateTime LastUpdateTime { get; set; }

        [DataMember]
        [Required]
        public virtual Device Device { get; set; }

        public DeviceProperty()
        {
            ResultParameters = new List<DeviceParameter>();
            LastUpdateTime = DateTime.Now;
        }

    }


}