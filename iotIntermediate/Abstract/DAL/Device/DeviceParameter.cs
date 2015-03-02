using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using iotDash.DAL.Device;
using iotDash.DAL.Device.Proprietary;

namespace iodash.Models.Common
{
    public class DeviceParameter
    {
        [Key]
        [Required]
        public int ParameterId { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public virtual ParameterType Type { get; set; }

        public string ParamDescription { get; set; }

        public string VisualRepresentationUrl { get; set; }

        public virtual DeviceAction Action { get; set; }

        public virtual DeviceProperty Property { get; set; }

        public sconnConfigMapper sconnMapper { get; set; }


    }
}