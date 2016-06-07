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
    public class sconnDeviceConfig : IAlarmSystemNamedConfigurationEntity, ISerializableConfiguration,
        IFakeAbleConfiguration, IIndexAbleConfiguration
    {
        public sconnDevice Device { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnDeviceConfig(byte Id)
        {
            Device = new sconnDevice {Id = Id};
        }

        public sconnDeviceConfig()
        {
            Device = new sconnDevice();
            UUID = Guid.NewGuid().ToString();
        }

        public byte[] Serialize()
        {
            try
            {
                return Device.Serialize();
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
                this.Device = new sconnDevice(buffer);
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
                this.Device = zone;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

        public int GetIndex()
        {
            return Device.Id;
        }

        public byte[] SerializeNames()
        {
            try
            {
                return Device.SerializeNames();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public void DeserializeNames(byte[] buffer)
        {
            try
            {
                this.Device.DeserializeNames(buffer);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);

            }
        }

        public string UUID { get; set; }
    }

}
