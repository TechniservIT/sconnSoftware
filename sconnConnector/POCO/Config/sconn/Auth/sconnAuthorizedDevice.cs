using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnAuthorizedDevice : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public int Id { get; set; }
        public string _Serial { get; set; }
        public bool _Enabled { get; set; }
        public DateTime _AllowedFrom { get; set; }
        public DateTime _AllowedUntil { get; set; }
        public int Value { get; set; }

        public sconnAuthorizedDevice()
        {
                
        }

        public sconnAuthorizedDevice(byte[] bytes)
        {
            this.Deserialize(bytes);       
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] bytes = new byte[ipcDefines.AUTH_RECORD_SIZE];
                char[] uuidBytes = _Serial.ToString().ToCharArray();
                for (int j = 0; j < uuidBytes.Length; j++)
                {
                    bytes[j] = (byte)uuidBytes[j];
                }
                bytes[ipcDefines.SYS_ALRM_DEV_ENABLED_POS] = (byte)(_Enabled ? 1 : 0);
                //TODO date range serialize/deserialize
                return bytes;
            }
            catch (Exception)
            {
                return new byte[0];
            }

        }

        static string ByteArrayToHexViaLookupAndShift(byte[] bytes)
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

        public void Deserialize(byte[] buffer)
        {
            try
            {
                // TODO date range
                // TODO enable
                string uuid;
                byte[] uuidBytes = new byte[ipcDefines.SYS_ALRM_UUID_LEN];
                for (int j = 0; j < ipcDefines.SYS_ALRM_UUID_LEN; j++)
                {
                    uuidBytes[j] = (byte)buffer[j];
                }
                uuid = ByteArrayToHexViaLookupAndShift(uuidBytes); //  Encoding.BigEndianUnicode.GetString(uuidBytes);
                if (uuid.Length != 0)
                {
                    _Serial = uuid;
                }
            }
            catch (Exception)
            {

            }

        }

        public void Fake()
        {
            this.Id = 0;
            this._Enabled = true;
            this._Serial = Guid.NewGuid().ToString();
        }

    }
}
