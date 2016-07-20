using System;
using System.Collections.Generic;
using NLog;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnAlarmZoneConfig : IAlarmSystemEntityConfig, IFakeAbleConfiguration
    {
        public List<sconnAlarmZone> Zones { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAlarmZoneConfig()
        {
                Zones = new List<sconnAlarmZone>();
            UUID = Guid.NewGuid().ToString();
        }

        public void Clear()
        {
            this.Zones = new List<sconnAlarmZone>();
        }

        public int GetEntityCount()
        {
            return Zones.Count;
        }

        public byte[] SerializeEntityWithId(int id)
        {
            try
            {
                return Zones[id].Serialize();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }
        public void DeserializeEntityWithId(byte[] buffer)
        {
            try
            {
                Zones.Add(new sconnAlarmZone(buffer));
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
                sconnAlarmZone zone = new sconnAlarmZone();
                zone.Fake();
                Zones.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
        

        public string UUID { get; set; }
        public byte[] SerializeEntityNames()
        {
            throw new NotImplementedException();
        }

        public void DeserializeEntityNames()
        {
            throw new NotImplementedException();
        }
    }

}
