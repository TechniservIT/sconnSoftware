using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public class sconnGsmConfig : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        public int RcptNo { get; set; }
        
        [MaxLength(ipcDefines.RAM_SMS_RECP_NO)]
        public List<sconnGsmRcpt> Rcpts { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public sconnGsmConfig()
        {
            Rcpts = new List<sconnGsmRcpt>();
        }

        public sconnGsmConfig(ipcSiteConfig cfg) :this()
        {
            this.Deserialize(cfg.GsmConfig);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] Serialized = new byte[ipcDefines.RAM_SMS_RECP_MEM_SIZE];
                for (int i = 0; i < Rcpts.Count; i++)
                {
                    byte[] partial = Rcpts[i].Serialize();
                    partial.CopyTo(Serialized, i * ipcDefines.RAM_SMS_RECP_SIZE);
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
                int relays = ipcDefines.RAM_SMS_RECP_NO;
                for (int i = 0; i < relays; i++)
                {
                    byte[] relayCfg = new byte[ipcDefines.RAM_SMS_RECP_SIZE];
                    for (int j = 0; j < ipcDefines.RAM_SMS_RECP_SIZE; j++)
                    {
                        relayCfg[j] = buffer[i * ipcDefines.RAM_SMS_RECP_SIZE+j];
                    }
                    sconnGsmRcpt relay = new sconnGsmRcpt(relayCfg);
                    relay.Id = i;
                    Rcpts.Add(relay);
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
                sconnGsmRcpt zone = new sconnGsmRcpt();
                zone.Fake();
                Rcpts.Add(zone);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

    }

}
