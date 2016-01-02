using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract
{

    public enum GsmMessagingLevel
    {
        All = 1,
        Violations,
        PowerChange,
        ArmChange,
        IoControl,
        ConfigChange,
        UserChange
    }

    public abstract class AlarmSystemGsmConfig
    {
        public  int RcptNo { get; set; }

        protected List<AlarmSystemGsmRcpt> Rcpts { get; set; }
        
    }


}
