using iotDash.DAL.Device.Proprietary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Common
{
    public class DeviceProperty
    {
        [Key]
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public string PropertyName { get; set; }

        public string  PropertyDescription { get; set; }

        public string VisualRepresentationURL { get; set; }

        public virtual ICollection<DeviceParameter> ResultParameters { get; set; }

        public DateTime LastUpdateTime { get; set; }

        [Required]
        public virtual Device Device { get; set; }


    }


}