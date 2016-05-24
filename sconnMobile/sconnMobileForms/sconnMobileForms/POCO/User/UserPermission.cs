using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iotDbConnector.DAL
{
    public class UserPermission
    {
        [Key]
        [Required]
        public int PerimissionId { get; set; }
    }
}