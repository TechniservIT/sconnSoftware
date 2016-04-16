using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;

namespace iotDash.Identity.Roles
{
    public class IotSiteManagerRole 
    {
        public virtual Site Site { get; set; }  
    }
}
