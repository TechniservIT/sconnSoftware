using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iotDbConnector.DAL
{
    public class AppAuthLevel
    {
        [Key]
        [Required]
        public int AppAuthLevelId { get; set; }

        public bool Write { get; set; }

        public bool Read { get; set; }

        public virtual ICollection<DeviceCredentials> Credentials { get; set; }

    }

}