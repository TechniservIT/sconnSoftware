using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sconnConnector.Config
{
    static public class CfgOper
    {

        static public int GetUnicodeArrayStringLen(byte[] arr)
        {
            int totalChars = 0;
            int UnicodeBytesForSign = 2;
            for (int i = 0; i < arr.Length / UnicodeBytesForSign; i += UnicodeBytesForSign)
            {
                if ((arr[i] == 0) && (arr[i + 1] == 0))
                {
                    return totalChars * UnicodeBytesForSign;
                }
                totalChars++;
            }

            return totalChars * UnicodeBytesForSign;
        }

        static public long GetLongFromBufferAtPos(byte[] buffer, int pos)
        {
            long res = 0;
            byte[] tmp = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tmp[i] = buffer[pos+i];
            }

            res = tmp[3];
            res |= (long)((long)tmp[2] << 8);
            res |= (long)((long)tmp[1] << 16);
            res |= (long)((long)tmp[0] << 24);

            return res;
        }

    }

}
