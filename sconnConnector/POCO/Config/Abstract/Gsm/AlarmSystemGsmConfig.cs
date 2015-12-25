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

    public abstract class AlarmSystemGsmConfig
    {
        public  int RcptNo { get; set; }

        protected List<AlarmSystemGsmRcpt> Rcpts { get; set; }
        
    }


}
