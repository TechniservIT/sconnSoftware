using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract
{
    public abstract class AlarmSystemGsmRcpt
    {
        public int CountryCode { get; set; }

        public string NumberE164 { get; set; }

        public  GsmMessagingLevel MessageLevel { get; set; }

    }


}
