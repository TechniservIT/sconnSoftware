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
        public string ActionName { get; set; }

        [DataMember]
        public string ActionDescription { get; set; }

        [DataMember]
        [Required]
        public virtual AIList<ActionParameter> RequiredParameters { get; set; }

        [DataMember]
        public string VisualRepresentationURL { get; set; }
        
        [DataMember]
        [Required]
        public virtual AIList<DeviceParameter> ResultParameters { get; set; }


        [DataMember]
        public DateTime LastActivationTime { get; set; }

        [DataMember]
        [Required]
        public virtual Device Device { get; set; }

        public DeviceAction()
        {
            RequiredParameters = new AIList<ActionParameter>();
            ResultParameters = new AIList<DeviceParameter>();

        }




    }

}