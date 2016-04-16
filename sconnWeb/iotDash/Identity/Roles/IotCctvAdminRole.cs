using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDash.Identity.Roles;
using iotDbConnector.DAL;

namespace iotDash.Identity.Roles
{
    public   class IotCctvAdminRole 
    {
        public virtual Site Site { get; set; }  
    }
}
