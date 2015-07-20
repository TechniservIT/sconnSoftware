using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDash.Models
{
    public class AlarmSystemSummaryModel
    {
        public int DeviceNo { get; set; }

        public AlarmSystemSummaryModel(int devno)
        {
            DeviceNo = devno;
        }
    }

}
