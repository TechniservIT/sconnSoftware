using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Device;

namespace sconnConnector.POCO.Config.sconn
{

    public class sconnDeviceConfig : IAlarmSystemNamedEntityConfig, IFakeAbleConfiguration, IIndexAbleConfiguration
    {
        public List<sconnDevice> Devices { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnDeviceConfig(byte Id)
        {
            Devices = new List<sconnDevice>();
        }

        public sconnDeviceConfig()
        {
            Devices = new List<sconnDevice>();
            UUID = Guid.NewGuid().ToString();
        }

        public void Clear()
        {
            this.Devices = new List<sconnDevice>();
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
                Devices.Add(new sconnDevice(buffer));
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
                sconnDevice zone = new sconnDevice();
                zone.Fake();
                this.Devices = new List<sconnDevice>();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

        public int GetIndex()
        {
            return 0;
        }
        
        public string UUID { get; set; }

        public byte[] SerializeEntityNames(int id)
        {
            return Devices[id].SerializeNames();
        }

        public void DeserializeEntityNames(int id, byte[] buffer)
        {
           Devices[id].DeserializeEntityNames(buffer);
        }
    }

}
