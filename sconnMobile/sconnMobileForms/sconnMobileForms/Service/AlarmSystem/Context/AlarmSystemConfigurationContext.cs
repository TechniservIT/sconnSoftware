using System;
using System.Collections.Generic;
using System.Text;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;
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
        static public AlarmDevicesConfigService DevicesService;

        static public EndpointInfo EndpointInfo;
        static public DeviceCredentials Credentials;

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

            AlarmSystemConfigManager man = new AlarmSystemConfigManager(EndpointInfo, Credentials);
            GlobalService = new GlobalConfigService(man);
            DevicesService = new AlarmDevicesConfigService(man);
        }

        static public GlobalConfigService GetGlobalConfigService()
        {
            return GlobalService;
        }

        static public AlarmDevicesConfigService GetDevicesConfigService()
        {
            return DevicesService;
        }

    }
}
