using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using NLog;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{

    [Serializable]
    public class sconnAuthorizedDevice : IAlarmSystemGenericConfigurationEntity
    {
        public ushort Id { get; set; }

        public string Serial { get; set; }
        public bool Enabled { get; set; }
        public DateTime AllowedFrom { get; set; }
        public DateTime AllowedUntil { get; set; }

        public ushort Value { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAuthorizedDevice()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public sconnAuthorizedDevice(byte[] bytes) : this()
        {
            this.Deserialize(bytes);       
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] bytes = new byte[ipcDefines.SYS_ALRM_DEV_AUTH_LEN];
                AlarmSystemConfig_Helpers.WriteWordToBufferAtPossition(Id, bytes, ipcDefines.SYS_ALRM_DEV_AUTH_ID_POS);
                char[] uuidBytes = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(Serial));
                int uuidStrLen = uuidBytes.Length > ipcDefines.SYS_ALRM_DEV_UUID_LEN
                    ? ipcDefines.SYS_ALRM_DEV_UUID_LEN
                    : uuidBytes.Length;
                for (int j = 0; j < uuidStrLen; j++)
                {
                    bytes[ipcDefines.SYS_ALRM_DEV_UUID_POS+j] = (byte)uuidBytes[j];
                }
                bytes[ipcDefines.SYS_ALRM_DEV_ENABLED_POS] = (byte)(Enabled ? 1 : 0);
                //TODO date range serialize/deserialize
                return bytes;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return new byte[0];
            }

        }

        static string ByteArrayToHexViaLookupAndShift(byte[] bytes)
        {
            try
            {
                StringBuilder result = new StringBuilder(bytes.Length * 2);
                string hexAlphabet = "0123456789ABCDEF";
                foreach (byte b in bytes)
                {
                    result.Append(hexAlphabet[(int)(b >> 4)]);
                    result.Append(hexAlphabet[(int)(b & 0xF)]);
                }
                return result.ToString();
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
                Id = (ushort)AlarmSystemConfig_Helpers.GetWordFromBufferAtPossition(buffer, ipcDefines.SYS_ALRM_DEV_AUTH_ID_POS);
                byte[] uuidBytes = new byte[ipcDefines.SYS_ALRM_DEV_UUID_LEN];
                for (int j = 0; j < ipcDefines.SYS_ALRM_DEV_UUID_LEN; j++)
                {
                    uuidBytes[j] = (byte)buffer[ipcDefines.SYS_ALRM_DEV_UUID_POS+j];
                }
                var uuid = Encoding.ASCII.GetString(uuidBytes,0, ipcDefines.SYS_ALRM_DEV_UUID_LEN);
                if (uuid.Length != 0)
                {
                    Serial = uuid;
                }
                this.Enabled = buffer[ipcDefines.SYS_ALRM_DEV_ENABLED_POS] > 0 ? true : false;
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
                this.Enabled = true;
                this.Serial = Guid.NewGuid().ToString().Substring(0, ipcDefines.SYS_ALRM_DEV_UUID_LEN);
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

        public void CopyFrom(sconnAuthorizedDevice other)
        {
            this.UUID = other.UUID;
            this.AllowedFrom = other.AllowedFrom;
            this.AllowedUntil = other.AllowedUntil;
            this.Enabled = other.Enabled;
            this.Id = other.Id;
            this.Serial = other.Serial;
        }

        public override bool Equals(object source)
        {
            CompareLogic compareLogic = new CompareLogic();
            ComparisonResult result = compareLogic.Compare(this, source);
            return result.AreEqual;
        }


    }
}
