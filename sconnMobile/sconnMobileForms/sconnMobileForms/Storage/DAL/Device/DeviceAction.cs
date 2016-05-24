using System;
using System.Collections.Generic;
using System.Linq;
 
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
 

namespace iotDbConnector.DAL
{

    public class DeviceAction 
    {

         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        [DisplayName("Name")]
        public string ActionName { get; set; }

         
        [DisplayName("Description")]
        public string ActionDescription { get; set; }

         
        [Required]
        public virtual List<ActionParameter> RequiredParameters { get; set; }

         
        public string VisualRepresentationURL { get; set; }
        
         
        [Required]
        public virtual List<DeviceActionResult> ResultParameters { get; set; }


         
        [DisplayName("Last Activation")]
        public DateTime LastActivationTime { get; set; }

         
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