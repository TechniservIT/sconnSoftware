using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract
{
    public class AlarmSystemDevice
    {
        public string Serial { get; set; }

        public bool Enabled { get; set; }

        public DateTime AllowedFrom { get; set; }

        public DateTime AllowedUntil { get; set; }
    }


}
