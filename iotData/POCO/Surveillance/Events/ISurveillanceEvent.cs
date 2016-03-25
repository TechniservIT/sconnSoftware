using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Events
{

    public interface ISurveillanceEvent
    {
        IpCamera Source { get; set; }
        DateTime Time { get; set; }
    }

}
