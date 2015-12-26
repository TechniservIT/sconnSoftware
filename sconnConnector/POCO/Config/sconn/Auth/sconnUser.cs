using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract.Auth
{
    public class sconnUser
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public int AuthLevel { get; set; }

        public int Type { get; set; }

        public bool Enabled { get; set; }

        public DateTime AllowedFrom { get; set; }

        public DateTime AllowedUntil { get; set; }
    }
}
