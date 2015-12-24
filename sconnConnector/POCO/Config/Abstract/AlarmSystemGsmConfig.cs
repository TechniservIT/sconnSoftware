using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract
{

    public enum GsmMessagingLevel
    {
        
    }

    public class AlarmSystemGsmConfig
    {
        public int RcptNo { get; set; }

        public List<AlarmSystemGsmRcpt> Rcpts { get; set; }
        
    }


}
