using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector;
using sconnConnector.Config;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.Abstract.Device;
using sconnConnector.POCO.Config.sconn;

namespace iotDash.Models
{


    public class AlarmSystemEventsModel : IAsyncStatusModel
    {
        public List<sconnEvent> Events { get; set; }

        public AlarmSystemEventsModel(List<sconnEvent> events)
        {
            this.Events = events;
        }

        public string Result { get; set; }
    }


    #region AuthorizedDevices

    public class AlarmSystemAuthorizedDevicesModel : IAsyncStatusModel
    {
        public List<sconnAuthorizedDevice>  AuthorizedDevices { get; set; }
        
        public AlarmSystemAuthorizedDevicesModel(List<sconnAuthorizedDevice> authcfg)
        {
            this.AuthorizedDevices = authcfg;
        }

        public string Result { get; set; }
    }

    public class AlarmSystemAddAuthorizedDeviceModel
    {
        [Required]
        [DisplayName("Authorizd Device")]
        public sconnAuthorizedDevice AuthorizedDevice { get; set; }
        public string Result { get; set; }

        public AlarmSystemAddAuthorizedDeviceModel()
        {

        }

        public AlarmSystemAddAuthorizedDeviceModel(sconnAuthorizedDevice Device)
        {
            this.AuthorizedDevice = Device;
        }
    }

    #endregion

    #region GsmConfig

    public class AlarmSystemGsmConfigModel : IAsyncStatusModel
    {
        public List<sconnGsmRcpt>  GsmRcpts { get; set; }
        public string Result { get; set; }

        public AlarmSystemGsmConfigModel()
        {
                
        }

        public AlarmSystemGsmConfigModel(List<sconnGsmRcpt> cfg)
        {
            GsmRcpts = cfg;
        }
    }

    public class AlarmSystemGsmAddRcptModel : IAsyncStatusModel
    {
        [Required]
        [DisplayName("Recipient")]
        public  sconnGsmRcpt GsmRcpt { get; set; }

        public AlarmSystemGsmAddRcptModel()
        {
                
        }

        public AlarmSystemGsmAddRcptModel(sconnGsmRcpt rcpt)
        {
            this.GsmRcpt = rcpt;
        }

        public string Result { get; set; }
    }

    #endregion


    #region ZoneConfig

    public class AlarmSystemZoneConfigModel : IAsyncStatusModel
    {
        public List<sconnAlarmZone> ZoneConfigs { get; set; }

        [Required]
        [DisplayName("Device")]
        public int DeviceId { get; set; }

        public AlarmSystemZoneConfigModel(List<sconnAlarmZone> zoneCfg)
        {
            ZoneConfigs = zoneCfg;
        }

        public AlarmSystemZoneConfigModel()
        {
                
        }

        public AlarmSystemZoneConfigModel(int DeviceId)
        {
            this.DeviceId = DeviceId;
        }

        public string Result { get; set; }
    }

    public class AlarmSystemZoneAddModel : IAsyncStatusModel
    {
        [Required]
        [DisplayName("Zone")]
        public sconnAlarmZone Zone { get; set; }

        public AlarmSystemZoneAddModel()
        {
                
        }
        
        public AlarmSystemZoneAddModel(sconnAlarmZone zone)
        {
            this.Zone = zone;
        }

        public string Result { get; set; }
    }


    #endregion

    #region 

    public class AlarmSystemUserConfigModel : IAsyncStatusModel
    {
        public List<sconnUser> Users { get; set; }

        public AlarmSystemUserConfigModel(List<sconnUser> userCfg)
        {
            Users = userCfg;
        }

        public string Result { get; set; }
    }

    public class AlarmSystemUserAddModel : IAsyncStatusModel
    {
        [Required]
        [DisplayName("User")]
        public sconnUser User { get; set; }

        public AlarmSystemUserAddModel()
        {

        }

        public AlarmSystemUserAddModel(sconnUser user)
        {
            this.User = user;
        }

        public string Result { get; set; }
    }

    #endregion


    #region NamesConfig

    public class AlarmSystemNamesConfigureModel : IAsyncStatusModel
    {
        public string Result { get; set; }
    }

    #endregion

    public class AlarmSystemSchedulesConfigureModel : IAsyncStatusModel
    {
        public string Result { get; set; }
    }
}
