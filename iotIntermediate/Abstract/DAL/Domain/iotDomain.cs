using iotDash.Models;
using iodash.Models.Application;
using iodash.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iotDash.DAL.Domain
{
    public class iotDomain
    {

        [Key]
        [Required]
        public int DomainId { get; set; }

        [Required]
        public string DomainName { get; set; }

        /*
        [Required]
        public virtual ApplicationUser Admin { get; set; }
        */

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Site> Sites { get; set; }


        public iotDomain()
        {

        }

        public iotDomain(string Name)
        {
            DomainName = Name;
        }

        public iotDomain(string Name, ApplicationUser admin) :this(Name)
        {
          
        }

    }
}