using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnGlobalConfig
    {
        private byte[] _memCFG;
        public byte[] memCFG
        {
            get { return _memCFG; }
            set { if (value != null) { _memCFG = value; } }
        }
        public sconnGlobalConfig()
        {
            _memCFG = new byte[ipcDefines.ipcGlobalConfigSize];
        }
    }

}
