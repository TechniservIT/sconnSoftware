using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{

    public class ipcEvent
    {
        public short EventNo { get; set; }
        public byte[] Buffer { get; set; }

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int Type { get; set; }

        public ipcEvent()
        {

        }

        public ipcEvent(byte[] EventBytes) : this()
        {
            //decode
            this.Buffer = EventBytes;
        }

    }
}
