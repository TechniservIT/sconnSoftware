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
    public class sconnGsmRcpt : IAlarmSystemGenericConfigurationEntity
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }
         
        public int CountryCode { get; set; }

        [MaxLength(ipcDefines.RAM_SMS_RECP_ADDR_LEN)]
        public string NumberE164 { get; set; }

        public GsmMessagingLevel MessageLevel { get; set; }

        public ushort Value { get; set; }

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
                AlarmSystemConfig_Helpers.WriteWordToBufferAtPossition(Id, Bytes, ipcDefines.RAM_SMS_RECP_ID_POS);
                Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS] = (byte)(CountryCode << 8);
                Bytes[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS + 1] = (byte)CountryCode;
                Bytes[ipcDefines.RAM_SMS_RECP_MESSAGE_LEVEL_POS] = (byte) MessageLevel;
                byte[] numbytes = (System.Text.Encoding.ASCII.GetBytes(NumberE164));
                int fullNumberLen = numbytes.Length > ipcDefines.RAM_SMS_RECP_ADDR_LEN 
                    ? ipcDefines.RAM_SMS_RECP_ADDR_LEN 
                    : numbytes.Length;
                for (int i = 0; i < fullNumberLen; i++)
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
                // this.Id = System.BitConverter.ToUInt16(buffer, ipcDefines.RAM_SMS_RECP_ID_POS);
                Id = (ushort)AlarmSystemConfig_Helpers.GetWordFromBufferAtPossition(buffer, ipcDefines.RAM_SMS_RECP_ID_POS);

                CountryCode = buffer[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS] << 8;
                CountryCode |= buffer[ipcDefines.RAM_SMS_RECP_COUNTRY_CODE_POS + 1];
                if (Enum.IsDefined(typeof (GsmMessagingLevel), (int)buffer[ipcDefines.RAM_SMS_RECP_MESSAGE_LEVEL_POS]))
                {
                    MessageLevel = (GsmMessagingLevel)((int)buffer[ipcDefines.RAM_SMS_RECP_MESSAGE_LEVEL_POS]);
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
        public byte[] SerializeEntityNames()
        {
            return new byte[0];
        }

        public void DeserializeEntityNames(byte[] buffer)
        {
            
        }

        public void CopyFrom(sconnGsmRcpt other)
        {
            this.UUID = other.UUID;
            this.CountryCode = other.CountryCode;
            this.Enabled = other.Enabled;
            this.Id = other.Id;
            this.MessageLevel = other.MessageLevel;
            this.Name = other.Name;
            this.NumberE164 = other.NumberE164;
            this.Value = other.Value;
        }

        public void Clone(IAlarmSystemConfigurationEntity other)
        {
            sconnGsmRcpt otherEntity = (sconnGsmRcpt)other;
            this.CopyFrom(otherEntity);
        }
    }
}
