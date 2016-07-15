using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public class sconnAuthorizedDevicesConfig : IAlarmSystemEntityConfig, IFakeAbleConfiguration
    {
        public List<sconnAuthorizedDevice>  Devices { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnAuthorizedDevicesConfig()
        {
            Devices = new List<sconnAuthorizedDevice>();
            UUID = Guid.NewGuid().ToString();
        }
        
        public void Fake()
        {
            sconnAuthorizedDevice zone = new sconnAuthorizedDevice();
            zone.Fake();
            Devices.Add(zone);
        }

        public void Clear()
        {
            this.Devices = new List<sconnAuthorizedDevice>();
        }

        public int GetEntityCount()
        {
            return Devices.Count;
        }

        public byte[] SerializeEntityWithId(int id)
        {
            try
            {
                return Devices[id].Serialize();
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
                Devices.Add(new sconnAuthorizedDevice(buffer));
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
        

        public string UUID { get; set; }
    }
}
