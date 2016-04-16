using iodash.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iotDash.DAL.Device.Proprietary
{
    public class sconnConfigMapper
    {
        [Key]
        [Required]
        public int MapperId { get; set; }

        [Required]
        public int ConfigType { get; set; }

        [Required]
        public int SeqNumber { get; set; }

        public virtual DeviceProperty Property { get; set; }

        public virtual DeviceAction Action { get; set; }
    }

}