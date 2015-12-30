using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnAuthorizedDevice 
    {
        public int Id { get; set; }
        public string _Serial;
        public bool _Enabled;
        public DateTime _AllowedFrom;
        public DateTime _AllowedUntil;


    }
}
