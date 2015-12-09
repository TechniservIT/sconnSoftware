using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector;
using sconnConnector.Config;
using iotDbConnector.DAL;

namespace iotDash.Models
{
  
    public class AlarmSystemInputsConfigureModel
    {
        public int DeviceId { get; set; }

        public int ServerId { get; set; }

        public List<sconnInput> Inputs { get; set; }

        public AlarmSystemInputsConfigureModel(Device dev, int DevId)
        {
            AlarmSystemConfigManager Config = new AlarmSystemConfigManager(dev.EndpInfo, dev.Credentials);
            Config.LoadSiteConfig();
            Inputs = Config.site.siteCfg.deviceConfigs[DevId].Inputs;
            DeviceId = DevId;
            ServerId = dev.Id;
        }
        public AlarmSystemInputsConfigureModel(Device dev, int DevId, AlarmSystemConfigManager man)
        {
            man.LoadSiteConfig();
            Inputs = man.site.siteCfg.deviceConfigs[DevId].Inputs;
            DeviceId = DevId;
            ServerId = dev.Id;
        }
    }

    public class AlarmSystemOutputsConfigureModel
    {
        public int DeviceId { get; set; }

        public int ServerId { get; set; }

        public List<sconnOutput> Outputs { get; set; }

        public AlarmSystemOutputsConfigureModel(Device dev, int DevId)
        {
            AlarmSystemConfigManager Config = new AlarmSystemConfigManager(dev.EndpInfo, dev.Credentials);
            Config.LoadSiteConfig();
            Outputs = Config.site.siteCfg.deviceConfigs[DevId].Outputs;
            DeviceId = DevId;
            ServerId = dev.Id;
        }

        public AlarmSystemOutputsConfigureModel(Device dev, int DevId, AlarmSystemConfigManager man)
        {
            man.LoadSiteConfig();
            Outputs = man.site.siteCfg.deviceConfigs[DevId].Outputs;
            DeviceId = DevId;
            ServerId = dev.Id;
        }
    }


    public class AlarmSystemRelaysConfigureModel
    {
        public int DeviceId { get; set; }

        public int ServerId { get; set; }
        public List<sconnRelay> Relays { get; set; }

        public AlarmSystemRelaysConfigureModel(Device dev, int DevId)
        {
            AlarmSystemConfigManager Config = new AlarmSystemConfigManager(dev.EndpInfo, dev.Credentials);
            Config.LoadSiteConfig();
            Relays = Config.site.siteCfg.deviceConfigs[DevId].Relays;
            DeviceId = DevId;
            ServerId = dev.Id;
        }

        public AlarmSystemRelaysConfigureModel(Device dev, int DevId, AlarmSystemConfigManager man)
        {
            man.LoadSiteConfig();
            Relays = man.site.siteCfg.deviceConfigs[DevId].Relays;
            DeviceId = DevId;
            ServerId = dev.Id;
        }
    }


    public class AlarmSystemNamesConfigureModel
    {

    }

    public class AlarmSystemSchedulesConfigureModel
    {

    }
}
