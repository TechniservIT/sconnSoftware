using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.IO;

namespace sconnConnector.POCO.Config.Abstract
{
    public abstract class AlarmSystemAuthorizedDevice
    {
        public abstract string Serial { get; set; }

        public abstract bool Enabled { get; set; }

        public abstract DateTime AllowedFrom { get; set; }

        public abstract  DateTime AllowedUntil { get; set; }

        
    }


}
