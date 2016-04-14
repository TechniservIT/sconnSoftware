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


        #region AlarmSystemEntitiesView

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

        #endregion



        #region AlarmSystemEntitiesConfig

            public const String AlarmConfig_Contract_Input_Config_View = "AlarmInputConfigureView";
            public const String AlarmConfig_Contract_Output_Config_View = "AlarmOutputConfigureView";
            public const String AlarmConfig_Contract_Relay_Config_View = "AlarmRelayConfigureView";
            public const String AlarmConfig_Contract_Zone_Config_View = "AlarmZoneConfigureView";
            public const String AlarmConfig_Contract_Sensor_Config_View = "AlarmSensorConfigureView";
            public const String AlarmConfig_Contract_Device_Config_View = "AlarmDeviceConfigureView";

            public static Uri AlarmUri_Config_Input_View = new Uri(AlarmConfig_Contract_Input_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Output_View = new Uri(AlarmConfig_Contract_Output_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Relay_View = new Uri(AlarmConfig_Contract_Relay_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Zone_View = new Uri(AlarmConfig_Contract_Zone_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Sensor_View = new Uri(AlarmConfig_Contract_Sensor_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_View = new Uri(AlarmConfig_Contract_Device_Config_View, UriKind.Relative);

        #endregion



        #region AlarmDevicesViews


            public const String AlarmStatus_Contract_Device_InputsModule_View = "AlarmDeviceInputsModuleView";
            public const String AlarmStatus_Contract_Device_Keypad_View = "AlarmDeviceKeypadView";
            public const String AlarmStatus_Contract_Device_Motherboard_View = "AlarmDeviceMotherboardView";
            public const String AlarmStatus_Contract_Device_Sensor_View = "AlarmDeviceSensorView";
            public const String AlarmStatus_Contract_Device_RelayModule_View = "AlarmSensorStatusureView";
            public const String AlarmStatus_Contract_Device_Siren_View = "AlarmDeviceSirenView";

            public static Uri AlarmUri_Status_Device_InputsModule_View = new Uri(AlarmStatus_Contract_Device_InputsModule_View, UriKind.Relative);
            public static Uri AlarmUri_Status_Device_Keypad_View = new Uri(AlarmStatus_Contract_Device_Keypad_View, UriKind.Relative);
            public static Uri AlarmUri_Status_Device_Motherboard_View = new Uri(AlarmStatus_Contract_Device_Motherboard_View, UriKind.Relative);
            public static Uri AlarmUri_Status_Device_Sensor_View = new Uri(AlarmStatus_Contract_Device_Sensor_View, UriKind.Relative);
            public static Uri AlarmUri_Status_Device_RelaysModule_View = new Uri(AlarmStatus_Contract_Device_RelayModule_View, UriKind.Relative);
            public static Uri AlarmUri_Status_Device_Siren_View = new Uri(AlarmStatus_Contract_Device_Siren_View, UriKind.Relative);


        #endregion



        #region AlarmDevicesConfigure

            public const String AlarmConfig_Contract_Device_InputsModule_View = "AlarmDeviceInputsModuleConfigureView";
            public const String AlarmConfig_Contract_Device_Keypad_View = "AlarmDeviceKeypadConfigureView";
            public const String AlarmConfig_Contract_Device_Motherboard_View = "AlarmDeviceMotherboardConfigureView";
            public const String AlarmConfig_Contract_Device_Sensor_View = "AlarmDeviceSensorConfigureView";
            public const String AlarmConfig_Contract_Device_RelayModule_View = "AlarmDeviceRelaysModuleConfigureView";
            public const String AlarmConfig_Contract_Device_Siren_View = "AlarmDeviceSirenConfigureView";

            public static Uri AlarmUri_Config_Device_InputsModule_View = new Uri(AlarmConfig_Contract_Device_InputsModule_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_Keypad_View = new Uri(AlarmConfig_Contract_Device_Keypad_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_Motherboard_View = new Uri(AlarmConfig_Contract_Device_Motherboard_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_Sensor_View = new Uri(AlarmConfig_Contract_Device_Sensor_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_RelaysModule_View = new Uri(AlarmConfig_Contract_Device_RelayModule_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_Siren_View = new Uri(AlarmConfig_Contract_Device_Siren_View, UriKind.Relative);


        #endregion

        #region AlarmManagment

            public const String AlarmStatus_Contract_Device_List_View = "AlarmSystemDeviceListView";
            public static Uri AlarmUri_Status_Device_List_View = new Uri(AlarmStatus_Contract_Device_List_View, UriKind.Relative);
        
        #endregion


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
