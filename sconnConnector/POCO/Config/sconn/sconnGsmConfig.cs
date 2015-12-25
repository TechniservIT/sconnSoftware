using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config
{
    public class sconnGsmConfig
    {
        public int RcptNo { get; set; }
        public List<AlarmSystemGsmRcpt> Rcpts { get; set; }
    }

}
