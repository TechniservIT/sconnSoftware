using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnGsmRcpt 
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public int CountryCode { get; set; }
        public string NumberE164 { get; set; }
        public GsmMessagingLevel MessageLevel { get; set; }

        public byte[] Serialized
        {
            get
            {
                //serialize
                byte[] Bytes = new byte[ipcDefines.RAM_SMS_RECP_SIZE];

                Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS] = (byte)(CountryCode << 8);
                Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS + 1] = (byte)CountryCode;

                byte[] numbytes = (System.Text.Encoding.ASCII.GetBytes(NumberE164));
                for (int i = 0; i < numbytes.Length; i++)
                {
                    Bytes[ipcDefines.RAM_SMS_RECP_ADDR_POS + i] = numbytes[i];
                }

                Bytes[ipcDefines.RAM_SMS_RECP_ENABLED_POS] = (byte)(Enabled == true ? 1 : 0);

                return Bytes;
            }
            set
            {

            }
        }

        public sconnGsmRcpt()
        {

        }

        public sconnGsmRcpt(ipcRcpt rcpt)
        {
            this.MessageLevel = rcpt.MessageLevel;
            this.CountryCode = rcpt.CountryCode;
            this.Enabled = rcpt.Enabled;
            this.NumberE164 = rcpt.NumberE164;
        }

        public sconnGsmRcpt(byte[] Bytes): this()
        {
            //decode
            CountryCode = Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS] << 8;
            CountryCode |= Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS + 1];

            byte[] NumberBytes = new byte[ipcDefines.RAM_SMS_RECP_ADDR_LEN];
            for (int i = 0; i < ipcDefines.RAM_SMS_RECP_ADDR_LEN; i++)
            {
                NumberBytes[i] = Bytes[ipcDefines.RAM_SMS_RECP_ADDR_POS + i];
            }
            NumberE164 = (System.Text.Encoding.ASCII.GetString(NumberBytes));
            Enabled = Bytes[ipcDefines.RAM_SMS_RECP_ENABLED_POS] == 1 ? true : false;

        }

    }
}
