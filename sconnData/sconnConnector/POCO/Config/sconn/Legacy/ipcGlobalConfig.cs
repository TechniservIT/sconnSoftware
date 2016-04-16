using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{

    public class ipcGlobalConfig
    {
        private byte[] _memCFG;
        public byte[] memCFG
        {
            get { return _memCFG; }
            set { if (value != null) { _memCFG = value; } }
        }
        public ipcGlobalConfig()
        {
            _memCFG = new byte[ipcDefines.ipcGlobalConfigSize];
        }
    }

}
