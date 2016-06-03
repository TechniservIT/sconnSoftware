using System;
using System.Collections.Generic;
using System.Text;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using AlarmSystemManagmentService.Event;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;
using sconnMobileForms.Service.AlarmSystem.Io;
using SiteManagmentService;

namespace sconnMobileForms.Service.AlarmSystem.Context
{
    static public class AlarmSystemConfigurationContext
    {

        static private sconnSite _site;
        static public sconnSite Site
        {
            get { return _site; }
            set
            {
                _site = value;
                ReloadServices();
            }
        }

        static public ISiteRepository Repository;

        static public sconnGlobalConfig GlobalConfig;
        static public List<sconnDeviceConfig> DeviceConfigs;
        static public List<sconnAlarmZone> Zones;

        static public GlobalConfigService GlobalService;
        static public GsmConfigurationService GsmService;
        static public ZoneConfigurationService ZoneService;
        static public UsersConfigurationService UsersService;
        static public EventsService EventsService;
        static public AlarmDevicesConfigService DevicesService;
    
        static public EndpointInfo EndpointInfo;
        static public DeviceCredentials Credentials;
        static public AlarmSystemConfigManager AlarmConfigManager;

        static private void ReloadServices()
        {
            EndpointInfo = new EndpointInfo
            {
                Hostname = Site.serverIP,
                Port = Site.serverPort
            };

            Credentials = new DeviceCredentials
            {
                Password = Site.authPasswd,
                Username = Site.authPasswd
            };

            AlarmConfigManager = new AlarmSystemConfigManager(EndpointInfo, Credentials);
            GlobalService = new GlobalConfigService(AlarmConfigManager);
            DevicesService = new AlarmDevicesConfigService(AlarmConfigManager);
            ZoneService = new ZoneConfigurationService(AlarmConfigManager);
            GsmService = new GsmConfigurationService(AlarmConfigManager);
            UsersService = new UsersConfigurationService(AlarmConfigManager);
        }

        static public GlobalConfigService GetGlobalConfigService()
        {
            return GlobalService;
        }

        static public AlarmDevicesConfigService GetDevicesConfigService()
        {
            return DevicesService;
        }

        static public IAlarmIoConfigService GetIoServiceForDeviceOutput(sconnDevice device, sconnOutput output)
        {
            return new AlarmIoOutputConfigService();
        }

    }
}
