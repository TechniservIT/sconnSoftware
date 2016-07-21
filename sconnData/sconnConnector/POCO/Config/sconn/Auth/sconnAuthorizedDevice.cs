﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
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
                char[] uuidBytes = _Serial.ToCharArray();
                int uuidStrLen = uuidBytes.Length > ipcDefines.SYS_ALRM_DEV_UUID_LEN
                    ? ipcDefines.SYS_ALRM_DEV_UUID_LEN
                    : uuidBytes.Length;
                for (int j = 0; j < uuidStrLen; j++)
                {
                    bytes[j] = (byte)uuidBytes[j];
                }
                bytes[ipcDefines.SYS_ALRM_DEV_ENABLED_POS] = (byte)(_Enabled ? 1 : 0);
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
                // TODO date range
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
                this._Enabled = buffer[ipcDefines.SYS_ALRM_DEV_ENABLED_POS] > 0 ? true : false;
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
                this._Enabled = true;
                this._Serial = Guid.NewGuid().ToString();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public string UUID { get; set; }
    }
}
