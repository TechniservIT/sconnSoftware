using iodash.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iotDash.DAL.Device
{
    public class ParameterType
    {
        [Key]
        [Required]
        public int ParameterId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string DocumentationURL { get; set; }


        public virtual ICollection<DeviceParameter> Parameters { get; set; }


        public string ParseTypeFromString(string typeval)
        {
            string parsed = "";
            return parsed;
        }

    }
}