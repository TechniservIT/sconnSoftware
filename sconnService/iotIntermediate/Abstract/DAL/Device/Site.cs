using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iodash.Models.Common;
using System.ComponentModel.DataAnnotations;
using iotDash.DAL.Domain;

namespace iodash.Models.Common
{
    public class Site
    {
        [Key]
        [Required]
        public int SiteId { get; set; }

        [Required]
        public string SiteName { get; set; }

        public virtual Location siteLocation { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        [Required]
        public virtual iotDomain Domain { get; set; }

    }
}