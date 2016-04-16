﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iotDash.Identity.Roles
{

    public enum IotUserRoleType
    {
        AlarmSystemAdmin = 1,
        CctvAdmin,
        DeviceManager,
        GlobalStatusViewer,
        MapAndTrackingAdmin,
        ScheduledSiteAdmin,
        SiteManager,
        DomainAdmin,
        IotRemoteDevice,
        IotDomainAdmin

    }


    public class IotUserRole : IdentityRole
    {
        [Required]
        [DisplayName("Domain")]
        public  int DomainId { get; set; }

        [Required]
        [DisplayName("Site")]
        public  int SiteId { get; set; }

        [Required]
        [DisplayName("Device")]
        public  int DeviceId { get; set; }  

        [Required(ErrorMessage = "Please select activation status.")]
        [Display(Name = "Active")]
        public  bool Active { get; set; }

        [Required]
        [DisplayName("Valid From")]
        public  DateTime ValidFrom { get; set; }

        [Required]
        [DisplayName("Valid Until")]
        public  DateTime ValidUntil { get; set; }

        [Required(ErrorMessage = "Please select role type.")]
        [Display(Name = "Type")]
        public IotUserRoleType Type { get; set; }

        public IotUserRole()
        {
            this.ValidFrom = DateTime.Now;
            this.ValidUntil = DateTime.MaxValue;
        }

        public IotUserRole(int domainId, bool active, IotUserRoleType type) : this()
        {
            this.DomainId = domainId;
            this.Active = active;
            this.Type = type;
        }

    }

}
