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
            foreach (ipcRcpt t in cfg.gsmRcpts)
            {
                sconnGsmRcpt rcpt = new sconnGsmRcpt(t);
                Rcpts.Add((rcpt));
            }
        }
    }

}
