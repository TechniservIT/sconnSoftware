 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceType
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string TypeName { get; set; }

        [DataMember]
        public string TypeDescription { get; set; }

        [DataMember]
        public string VisualRepresentationURL { get; set; }

        [DataMember]
        public virtual AIList<Device> Devices { get; set; }

        [Required]
        [DataMember]
        public virtual iotDomain Domain { get; set; }

        public DeviceType()
        {
            Devices = new AIList<Device>();
        }
    }
}