using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Runtime.Serialization;
 

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class Device
    {
        [DataMember]
        [Required]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string DeviceName { get; set; }

        [DataMember]
        [Required]
        public virtual EndpointInfo EndpInfo { get; set; }

        [DataMember]
        public virtual DeviceCredentials Credentials { get; set; }

        [DataMember]
        public virtual List<DeviceAction> Actions { get; set; }

        [DataMember]
        public virtual List<DeviceProperty> Properties { get; set; }

        [DataMember]
        [Required]
        public virtual Location DeviceLocation { get; set; }

        [DataMember]
        [Required]
        public virtual DeviceType Type { get; set; }

        [DataMember]
        [Required]
        public virtual Site Site { get; set; }

        [DataMember]
        public bool IsVirtual { get; set; }


        public Device()
        {
            Actions = new List<DeviceAction>();
            Properties = new List<DeviceProperty>();
        }

       


    }
}