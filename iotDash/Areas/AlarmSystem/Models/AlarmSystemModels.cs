using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.Config;

namespace iotDash.Models
{
    public class AlarmSystemSummaryModel
    {
        public int DeviceNo { get; set; }

        public Device AlarmDevice { get; set; }

        public AlarmSystemSummaryModel(int devno, Device  dev)
        {
            DeviceNo = devno;
            AlarmDevice = dev;
        }
    }

    public class AlarmSystemDetailModel
    {

        public AlarmSystemConfigManager  Config { get; set; }

        public Device AlarmDevice { get; set; }

        public AlarmSystemDetailModel(Device dev)
        {
            //get config from DB
            AlarmDevice = dev;
            Config = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
            Config.LoadSiteConfig();

        }
    }


}
