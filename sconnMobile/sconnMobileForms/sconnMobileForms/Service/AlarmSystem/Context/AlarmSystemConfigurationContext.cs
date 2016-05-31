using System;
using System.Collections.Generic;
using System.Text;
using AlarmSystemManagmentService;
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
        static public sconnSite Site;

        static public ISiteRepository Repository;

        static public sconnGlobalConfig GlobalConfig;
        static public List<sconnDeviceConfig> DeviceConfigs;
        static public List<sconnAlarmZone> Zones;

        static public GlobalConfigService GetGlobalConfigService()
        {
            EndpointInfo info = new EndpointInfo();
            info.Hostname = Site.serverIP;
            info.Port = Site.serverPort;

            DeviceCredentials cred = new DeviceCredentials();
            cred.Password = Site.authPasswd;
            cred.Username = Site.authPasswd;
            
            AlarmSystemConfigManager man = new AlarmSystemConfigManager(info, cred);
            GlobalConfigService gs = new GlobalConfigService(man);

            return gs;
        }
    }
}
