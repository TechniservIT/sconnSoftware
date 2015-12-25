using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract.Event;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnEvent
    {
        public short EventNo { get; set; }

        public sconnEvent()
        {

        }

        public sconnEvent(byte[] EventBytes) : this()
        {
            //decode
        }
    }

}
