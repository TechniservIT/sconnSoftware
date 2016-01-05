using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class sconnAlarmZone : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AlarmZoneType Type { get; set; }
        public bool Enabled { get; set; }
        public int NameId { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAlarmZone()
        {
                
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
                //todo - load name
                Type = (AlarmZoneType)buffer[ipcDefines.ZONE_CFG_TYPE_POS];
                Enabled = buffer[ipcDefines.ZONE_CFG_ENABLED_POS] > 0 ? true : false;
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
                this.NameId = 0;
                this.Type = AlarmZoneType.General;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }
    }

}
