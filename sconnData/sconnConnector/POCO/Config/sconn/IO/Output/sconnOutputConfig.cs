using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sconnConnector.POCO.Config.sconn.IO.Output
{
    public class sconnOutputConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public List<sconnOutput> Outputs { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnOutputConfig()
        {
            Outputs = new List<sconnOutput>();
            UUID = Guid.NewGuid().ToString();
        }


        public sconnOutputConfig(ipcSiteConfig cfg) : this()
        {

        }

        public byte[] Serialize()
        {

            try
            {
                byte[] Serialized = new byte[ipcDefines.OutputsTotalMemSize];
                for (int i = 0; i < Outputs.Count; i++)
                {
                    byte[] partial = Outputs[i].Serialize();
                    partial.CopyTo(Serialized, i * ipcDefines.mAdrOutputMemSize);
                }
                return Serialized;
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
                byte outputs = buffer[ipcDefines.DeviceMaxOutputs];
                for (byte i = 0; i < outputs; i++)
                {
                    byte[] relayCfg = new byte[ipcDefines.mAdrOutputMemSize];
                    for (byte j = 0; j < ipcDefines.mAdrOutputMemSize; j++)
                    {
                        relayCfg[j] = buffer[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize + j];
                    }
                    sconnOutput relay = new sconnOutput(relayCfg);
                    relay.Id = (byte)i;
                    Outputs.Add(relay);
                }
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
                sconnOutput zone = new sconnOutput();
                zone.Fake();
                Outputs.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public string UUID { get; set; }
    }
}
