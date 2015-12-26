using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Device;

namespace sconnConnector.POCO.Config.sconn
{
    public class sconnDeviceConfig
    {
        public List<sconnDevice> Devices { get; set; }

        public sconnDeviceConfig()
        {
                this.Devices = new List<sconnDevice>();
        }

        public sconnDeviceConfig(ipcSiteConfig cfg)
        {
            Devices = new List<sconnDevice>();
            for (int i = 0; i < cfg.deviceNo; i++)
            {
                sconnDevice dev = new sconnDevice(cfg.deviceConfigs[i]);
                Devices.Add(dev);
            }
        }

    }

}
