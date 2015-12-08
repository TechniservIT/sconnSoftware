using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector;
using System.ComponentModel.DataAnnotations;

namespace iotDash.Models
{

    public class AlarmSystemListModel
    {
        public int DeviceNo { get; set; }

        public List<Device> AlarmDevices { get; set; }

        public AlarmSystemListModel(List<Device> devices)
        {
            AlarmDevices = devices;
        }
    }



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


    public class AlarmSystemInputEditModel
    {

        [Required]
        public int DeviceId { get; set; }

        public ipcDataType.ipcDeviceConfig DevCfg { get; set; }

        [Required]
        public List<sconnInput> Inputs { get; set; }

    }


    public class AlarmSystemDetailModel
    {

        [Required]
        public int ServerId { get; set; }

        [Required]
        public List<sconnInput> Inputs { get; set; }


        public AlarmSystemConfigManager Config { get; set; }

        public Device AlarmDevice { get; set; }

        public sconnConnector.ipcDataType.ipcDeviceConfig EditedDevice { get; set; }


        public AlarmSystemDetailModel(Device dev)
        {
            //get config from DB
            AlarmDevice = dev;
            ServerId = dev.Id;
            Config = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
            Config.LoadSiteConfig();
            EditedDevice = Config.site.siteCfg.deviceConfigs[0];
        }
    }


}
