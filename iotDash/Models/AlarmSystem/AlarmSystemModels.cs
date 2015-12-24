using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector;
using sconnConnector.Config;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract;

namespace iotDash.Models
{

    #region IoConfigure
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

    #endregion

    #region AuthorizedDevices

    public class AlarmSystemAuthorizedDevicesModel
    {
        public List<AlarmSystemDevice>  AuthorizeDevices { get; set; }

        public AlarmSystemAuthorizedDevicesModel(List<AlarmSystemDevice> devices)
        {
            this.AuthorizeDevices = devices;
        }

    }

    public class AlarmSystemAddAuthorizedDeviceModel
    {
        public AlarmSystemDevice AuthorizedDevice { get; set; }  
    }

    #endregion

    #region GsmConfig

    public class AlarmSystemGsmConfigModel
    {
        public AlarmSystemGsmConfig GsmConfig { get; set; }
    }

    public class AlarmSystemGsmAddRcptModel
    {
        public AlarmSystemGsmRcpt GsmRcpt { get; set; }

        public AlarmSystemGsmAddRcptModel()
        {
                
        }

        public AlarmSystemGsmAddRcptModel(AlarmSystemGsmRcpt rcpt)
        {
            this.GsmRcpt = rcpt;
        }
    }

    #endregion


    #region ZoneConfig

    public class AlarmSystemZoneConfigModel
    {
        public List<AlarmSystemZone> Zones { get; set; }

        public AlarmSystemZoneConfigModel()
        {
                
        }
    }

    public class AlarmSystemZoneAddModel
    {
        public AlarmSystemZone Zone { get; set; }

        public AlarmSystemZoneAddModel()
        {
                
        }

        public AlarmSystemZoneAddModel(AlarmSystemZone zone)
        {
            this.Zone = zone;
        }

    }


    #endregion



    #region NamesConfig

    public class AlarmSystemNamesConfigureModel
    {

    }

    #endregion

    public class AlarmSystemSchedulesConfigureModel
    {

    }
}
