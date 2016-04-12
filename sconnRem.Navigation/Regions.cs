using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnRem.Navigation
{

    public static class AlarmRegionNames
    {
        public const String MainContentRegion = "MainContentRegion";
        public const String MainNavigationRegion = "MainNavigationRegion";

        public const String AlarmConfig_Contract_AuthConfigView = "AuthConfigView";
        public const String AlarmConfig_Contract_GlobalConfigView = "GlobalConfigView";
        public const String AlarmConfig_Contract_CommConfigView = "CommConfigView";
        public const String AlarmConfig_Contract_GsmConfigView = "GsmConfigView";
        public const String AlarmConfig_Contract_ZoneConfigView = "ZoneConfigView";
        public const String AlarmConfig_Contract_UsersConfigView = "UserConfigView";

        public const String AlarmStatus_Contract_InputsView = "AlarmInputsView";
        public const String AlarmStatus_Contract_OutputsView = "AlarmOutputsView";
        public const String AlarmStatus_Contract_RelaysView = "AlarmRelaysView";
        public const String AlarmStatus_Contract_ZonesView = "AlarmZonesView";
        public const String AlarmStatus_Contract_SensorsView = "AlarmSensorsView";
        public const String AlarmStatus_Contract_DeviceView = "AlarmDeviceView";

        public static Uri AlarmUri_Status_Inputs_View = new Uri(AlarmStatus_Contract_InputsView, UriKind.Relative);
        public static Uri AlarmUri_Status_Outputs_View = new Uri(AlarmStatus_Contract_OutputsView, UriKind.Relative);
        public static Uri AlarmUri_Status_Relays_View = new Uri(AlarmStatus_Contract_RelaysView, UriKind.Relative);
        public static Uri AlarmUri_Status_Zones_View = new Uri(AlarmStatus_Contract_ZonesView, UriKind.Relative);
        public static Uri AlarmUri_Status_Sensors_View = new Uri(AlarmStatus_Contract_SensorsView, UriKind.Relative);
        public static Uri AlarmUri_Status_Device_View = new Uri(AlarmStatus_Contract_DeviceView, UriKind.Relative);
        
        public static Uri AlarmUri_Config_Auth_View = new Uri("/View/Config/AlarmSystem/" + AlarmConfig_Contract_AuthConfigView, UriKind.Relative);
        public static Uri AlarmUri_Config_Global_View = new Uri("/View/Config/AlarmSystem/" + AlarmConfig_Contract_GlobalConfigView, UriKind.Relative);
        public static Uri AlarmUri_Config_Comm_View = new Uri("/View/Config/AlarmSystem/" + AlarmConfig_Contract_CommConfigView, UriKind.Relative);
        public static Uri AlarmUri_Config_Gsm_View = new Uri("/View/Config/AlarmSystem/" + AlarmConfig_Contract_GsmConfigView, UriKind.Relative);
        public static Uri AlarmUri_Config_Zone_View = new Uri("/View/Config/AlarmSystem/" + AlarmConfig_Contract_ZoneConfigView, UriKind.Relative);
        public static Uri AlarmUri_Config_Users_View = new Uri("/View/Config/AlarmSystem/" + AlarmConfig_Contract_UsersConfigView, UriKind.Relative);


    }

    public static class NavContextToolbarRegionNames
    {
        public const String ContextToolbar_AlarmSystem_ViewName = "AlarmSystemToolbarView";
        public static Uri ContextToolbar_AlarmSystem_ViewUri = new Uri(ContextToolbar_AlarmSystem_ViewName, UriKind.Relative);  //"/View/Menu/ContextToolbar/AlarmSystem/" + 
    }

    public static class GlobalViewRegionNames
    {
        public const String MainGridContentRegion = "MainViewGridRegion";

        public const String RNavigationRegion = "RightSideToolbarRegion";
        public const String LNavigationRegion = "LeftSideMenuRegion";
        public const String RopNavigationRegion = "TopToolbarRegion";

        public const String TopContextToolbarRegion = "TopContextToolbarRegion";
    }
    
}
