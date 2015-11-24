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

    }

}
