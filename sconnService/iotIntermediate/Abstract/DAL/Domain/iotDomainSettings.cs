using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.DAL.Domain
{
    public class iotDomainSettings
    {

        public int MaxConcurrentUsers { get; set; }

        public int MaxSites { get; set; }

        public bool UpdateSitesOnUserLogon { get; set; }
    
    }
}