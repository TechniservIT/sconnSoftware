using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnGsmRcpt : ISerializableConfiguration,  IFakeAbleConfiguration
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public int CountryCode { get; set; }

        [MaxLength(ipcDefines.RAM_SMS_RECP_ADDR_LEN)]
        public string NumberE164 { get; set; }

        public GsmMessagingLevel MessageLevel { get; set; }

        public int Value { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnGsmRcpt()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public sconnGsmRcpt(byte[] Bytes): this()
        {
            this.Deserialize(Bytes);
        }

        public byte[] Serialize()
        {
            try
            {
                //serialize
                byte[] Bytes = new byte[ipcDefines.RAM_SMS_RECP_SIZE];
                Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS] = (byte)(CountryCode << 8);
                Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS + 1] = (byte)CountryCode;
                Bytes[ipcDefines.RAM_SMS_RECP_MESSAGE_LEVEL_POS] = (byte) MessageLevel;
                byte[] numbytes = (System.Text.Encoding.ASCII.GetBytes(NumberE164));
                for (int i = 0; i < numbytes.Length; i++)
                {
                    Bytes[ipcDefines.RAM_SMS_RECP_ADDR_POS + i] = numbytes[i];
                }
                Bytes[ipcDefines.RAM_SMS_RECP_ENABLED_POS] = (byte)(Enabled == true ? 1 : 0);
                return Bytes;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                //decode
                CountryCode = buffer[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS] << 8;
                CountryCode |= buffer[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS + 1];
                if (Enum.IsDefined(typeof (GsmMessagingLevel), (int)buffer[ipcDefines.RAM_SMS_RECP_MESSAGE_LEVEL_POS]))
                {
                    MessageLevel = (GsmMessagingLevel)buffer[ipcDefines.RAM_SMS_RECP_MESSAGE_LEVEL_POS];
                }
                byte[] NumberBytes = new byte[ipcDefines.RAM_SMS_RECP_ADDR_LEN];
                for (int i = 0; i < ipcDefines.RAM_SMS_RECP_ADDR_LEN; i++)
                {
                    NumberBytes[i] = buffer[ipcDefines.RAM_SMS_RECP_ADDR_POS + i];
                }
                NumberE164 = (System.Text.Encoding.ASCII.GetString(NumberBytes));
                Enabled = buffer[ipcDefines.RAM_SMS_RECP_ENABLED_POS] == 1 ? true : false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }


        }

        public void Fake()
        {
            try
            {

                this.Id = 0;
                this.CountryCode = 48;
                this.Enabled = true;
                this.MessageLevel = GsmMessagingLevel.All;
                this.Name = Guid.NewGuid().ToString();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

        public string UUID { get; set; }
    }
}
