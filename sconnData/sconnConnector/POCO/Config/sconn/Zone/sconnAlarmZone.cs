using System;
using NLog;

namespace sconnConnector.POCO.Config.sconn
{
    public enum AlarmZoneType 
    {
        General = 1,
        Nightly,
        Z24h,
        IoControl,
        Time_Guarded
    }

    public interface IAlarmSystemNamedEntityWithType
    {
        string Name { get; set; }
        string imageIconUri { get; set; }
    }

    public class sconnAlarmZone : IAlarmSystemNamedEntity, ISerializableConfiguration, IFakeAbleConfiguration, IAlarmSystemNamedEntityWithType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AlarmZoneType Type { get; set; }
        public bool Enabled { get; set; }
        public int NameId { get; set; }

        public string imageIconUri { get; set; }
        public string imageRealUri { get; set; }


        public string GetDeviceTypeImageUriForDevice(sconnAlarmZone ev)
        {
            if (ev.Type == AlarmZoneType.General)
            {
                return "pack://application:,,,/images/klawiatura1000x1000.jpg";
            }
            else if (ev.Type == AlarmZoneType.IoControl)
            {
                return "pack://application:,,,/images/tel1000.jpg";
            }
            else if (ev.Type == AlarmZoneType.Nightly)
            {
                return "pack://application:,,,/images/tel1000.jpg";
            }
            else if (ev.Type == AlarmZoneType.General)
            {
                return "pack://application:,,,/images/tel1000.jpg";
            }
            else if (ev.Type == AlarmZoneType.Time_Guarded)
            {
                return "pack://application:,,,/images/tel1000.jpg";
            }
            else
            {
                return "pack://application:,,,/images/strefy2-1000.jpg";
            }
            
        }
        
        public void LoadImageTypeUrl()
        {
            imageIconUri = GetDeviceTypeImageUriForDevice(this);
        }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAlarmZone()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public sconnAlarmZone(byte[] serialized) : this()
        {
            this.Deserialize(serialized);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.ZONE_CFG_LEN];
                buffer[ipcDefines.ZONE_CFG_TYPE_POS] = (byte)Type;
                buffer[ipcDefines.ZONE_CFG_ENABLED_POS] = (byte)(Enabled ? 1 : 0);
                byte[] nameBytes = System.Text.Encoding.BigEndianUnicode.GetBytes(Name);
                for (int i = 0; i < nameBytes.Length; i++)
                {
                    buffer[ipcDefines.ZONE_CFG_NAME_POS + i] = nameBytes[i];
                }
                return buffer;
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
                if (Enum.IsDefined(typeof(AlarmZoneType), (int)buffer[ipcDefines.ZONE_CFG_TYPE_POS]))
                {
                    Type = (AlarmZoneType)buffer[ipcDefines.ZONE_CFG_TYPE_POS];
                }
                Enabled = buffer[ipcDefines.ZONE_CFG_ENABLED_POS] > 0 ? true : false;
                Name = System.Text.Encoding.BigEndianUnicode.GetString(
                    buffer, 
                    ipcDefines.ZONE_CFG_NAME_POS, 
                    AlarmSystemConfig_Helpers.GetUtf8StringLengthFromBufferAtPossition(buffer, ipcDefines.ZONE_CFG_NAME_POS));
                LoadImageTypeUrl();
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
                this.Name = Guid.NewGuid().ToString();
                UUID = Guid.NewGuid().ToString();
                this.NameId = 0;
                this.Type = AlarmZoneType.General;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public byte[] SerializeEntityNames()
        {
            return  System.Text.Encoding.BigEndianUnicode.GetBytes(Name);
        }

        public void DeserializeEntityNames(byte[] buffer)
        {
            Name = System.Text.Encoding.BigEndianUnicode.GetString(buffer, 0, buffer.GetLength(0));
        }

        public string UUID { get; set; }
    }

}
