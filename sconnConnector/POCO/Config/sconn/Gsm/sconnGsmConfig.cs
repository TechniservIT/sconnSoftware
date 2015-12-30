using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public class sconnGsmConfig
    {
        public int RcptNo { get; set; }
        public List<sconnGsmRcpt> Rcpts { get; set; }

        public sconnGsmConfig()
        {
            Rcpts = new List<sconnGsmRcpt>();
        }

        public sconnGsmConfig(ipcSiteConfig cfg) :this()
        {
            for (int i = 0; i < cfg.gsmRcpts.Length; i++)
            {
                sconnGsmRcpt rcpt = new sconnGsmRcpt(cfg.gsmRcpts[i]);
                rcpt.Id = i;
                Rcpts.Add((rcpt));
            }
        }
    }

}
