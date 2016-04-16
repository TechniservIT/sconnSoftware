using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using iotDash.DAL.Device.Proprietary;

namespace iodash.Models.Common
{
    public class DeviceAction
    {
        [Key]
        [Required]
        public int ActionId { get; set; }

        [Required]
        public string ActionName { get; set; }
     
        public string ActionDescription { get; set; }

        [Required]
        public virtual ICollection<DeviceParameter> RequiredParameters { get; set; }

        public string VisualRepresentationURL { get; set; }

        [Required]
        public virtual ICollection<DeviceParameter> ResultParameters { get; set; }

        public DateTime LastActivationTime { get; set; }

        [Required]
        public virtual Device Device { get; set; }

    }

}