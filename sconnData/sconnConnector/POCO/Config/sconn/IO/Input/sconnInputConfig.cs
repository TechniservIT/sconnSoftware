using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace sconnConnector.POCO.Config.sconn.IO
{
    public class sconnInputConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {

        public List<sconnInput> Inputs { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnInputConfig()
        {
            Inputs = new List<sconnInput>();
            UUID = Guid.NewGuid().ToString();
        }

        public sconnInputConfig(ipcSiteConfig cfg) : this()
        {
           
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] Serialized = new byte[ipcDefines.mAdrInputMemSize];
                for (int i = 0; i < Inputs.Count; i++)
                {
                    byte[] partial = Inputs[i].Serialize();
                    partial.CopyTo(Serialized, i * ipcDefines.mAdrInputMemSize);
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
                byte relays = buffer[ipcDefines.DeviceMaxInputs];
                for (byte i = 0; i < relays; i++)
                {
                    byte[] relayCfg = new byte[ipcDefines.mAdrInputMemSize];
                    for (byte j = 0; j < ipcDefines.mAdrInputMemSize; j++)
                    {
                        relayCfg[j] = buffer[ipcDefines.mAdrInput + i * ipcDefines.mAdrInputMemSize + j];
                    }
                    sconnInput relay = new sconnInput(relayCfg);
                    relay.Id = i;
                    Inputs.Add(relay);
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
                sconnInput zone = new sconnInput();
                zone.Fake();
                Inputs.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public string UUID { get; set; }
    }
}
