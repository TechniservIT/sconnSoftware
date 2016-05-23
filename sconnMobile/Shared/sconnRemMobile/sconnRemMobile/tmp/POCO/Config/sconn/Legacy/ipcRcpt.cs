using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config
{
    public class ipcRcpt : AlarmSystemGsmRcpt
    {
        
        public byte[] RawBytes { get; set; }
        
        public ipcRcpt()
        {
            RawBytes = new byte[ipcDefines.RAM_SMS_RECP_SIZE];
        }
        
    }

}
