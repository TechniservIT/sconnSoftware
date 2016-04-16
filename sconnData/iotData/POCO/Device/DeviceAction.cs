using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
 

namespace iotDbConnector.DAL
{
    [DataContract(IsReference = true)]
    public class DeviceAction 
    {

        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Name")]
        public string ActionName { get; set; }

        [DataMember]
        [DisplayName("Description")]
        public string ActionDescription { get; set; }

        [DataMember]
        [Required]
        public virtual List<ActionParameter> RequiredParameters { get; set; }

        [DataMember]
        public string VisualRepresentationURL { get; set; }
        
        [DataMember]
        [Required]
        public virtual List<DeviceActionResult> ResultParameters { get; set; }


        [DataMember]
        [DisplayName("Last Activation")]
        public DateTime LastActivationTime { get; set; }

        [DataMember]
        [Required]
        public virtual Device Device { get; set; }

        public DeviceAction()
        {
            RequiredParameters = new List<ActionParameter>();
            ResultParameters = new List<DeviceActionResult>();
            LastActivationTime = DateTime.Now;
        }

    }

}