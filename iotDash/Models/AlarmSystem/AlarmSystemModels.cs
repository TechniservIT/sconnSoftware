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
using sconnConnector.POCO.Config.Abstract.Device;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Models
{

    #region IoConfigure
    public class AlarmSystemInputsConfigureModel : IAsyncStatusModel
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

    public class AlarmSystemOutputsConfigureModel : IAsyncStatusModel
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


    public class AlarmSystemRelaysConfigureModel : IAsyncStatusModel
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

    public class AlarmSystemAuthorizedDevicesModel : IAsyncStatusModel
    {
        public sconnAuthorizedDevices  AuthorizedDevices { get; set; }
        
        public AlarmSystemAuthorizedDevicesModel(sconnAuthorizedDevices authcfg)
        {
            this.AuthorizedDevices = authcfg;
        }

    }

    public class AlarmSystemAddAuthorizedDeviceModel
    {
        public sconnAuthorizedDevice AuthorizedDevice { get; set; }
        public string Result { get; set; }
    }

    #endregion

    #region GsmConfig

    public class AlarmSystemGsmConfigModel : IAsyncStatusModel
    {
        public sconnGsmConfig GsmConfig { get; set; }
    }

    public class AlarmSystemGsmAddRcptModel : IAsyncStatusModel
    {
        public sconnGsmRcpt GsmRcpt { get; set; }

        public AlarmSystemGsmAddRcptModel()
        {
                
        }

        public AlarmSystemGsmAddRcptModel(sconnGsmRcpt rcpt)
        {
            this.GsmRcpt = rcpt;
        }
    }

    #endregion


    #region ZoneConfig

    public class AlarmSystemZoneConfigModel : IAsyncStatusModel
    {
        public sconnAlarmZoneConfig ZoneConfig { get; set; }

        public AlarmSystemZoneConfigModel(sconnAlarmZoneConfig zoneCfg)
        {
                
        }
    }

    public class AlarmSystemZoneAddModel : IAsyncStatusModel
    {
        public sconnAlarmZone Zone { get; set; }

        public AlarmSystemZoneAddModel()
        {
                
        }

        public AlarmSystemZoneAddModel(sconnAlarmZone zone)
        {
            this.Zone = zone;
        }

    }


    #endregion



    #region NamesConfig

    public class AlarmSystemNamesConfigureModel : IAsyncStatusModel
    {

    }

    #endregion

    public class AlarmSystemSchedulesConfigureModel : IAsyncStatusModel
    {

    }
}
