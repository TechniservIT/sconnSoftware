using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.Config
{
    static public class ConfigHelpers
    {
        static public  ushort WordFromBufferAtPos(byte[] buffer, int pos)
        {
            ushort val;
            val = buffer[pos+1];
            val |= (ushort) (buffer[pos] << 8);
            return val;
        }
    }
}
