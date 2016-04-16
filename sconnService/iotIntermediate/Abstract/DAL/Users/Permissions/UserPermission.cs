using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Application.Users
{
    public class UserPermission
    {
        [Key]
        [Required]
        public int PerimissionId { get; set; }
    }
}