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


        #region AlarmSystemEntitiesView

            public const String AlarmStatus_Contract_InputsView = "AlarmInputsViewContract";
            public const String AlarmStatus_Contract_OutputsView = "AlarmOutputsViewContract";
            public const String AlarmStatus_Contract_RelaysView = "AlarmRelaysViewContract";
            public const String AlarmStatus_Contract_ZonesView = "AlarmZonesViewContract";
            public const String AlarmStatus_Contract_SensorsView = "AlarmSensorsViewContract";
            public const String AlarmStatus_Contract_DeviceView = "AlarmDeviceViewContract";
            public const String AlarmStatus_Contract_EventsView = "AlarmEventsViewContract";
            public const String AlarmStatus_Contract_PowerView = "AlarmPowerViewContract";
            public const String AlarmStatus_Contract_NetworkView = "AlarmNetworkViewContract";
            public const String AlarmStatus_Contract_GsmRcpts = "AlarmGsmRcptsViewContract";
            public const String AlarmStatus_Contract_RemoteUsers = "AlarmRemoteUsersViewContract";
            public const String AlarmStatus_Contract_SystemUsers = "AlarmSystemusersViewContract";
            public const String AlarmStatus_Contract_AuthorizedDevices = "AlarmSystemAuthorizedDevicesViewContract";
            public const String AlarmStatus_Contract_TemperatureSensorsView = "AlarmTemperatureSensorsViewContract";
            public const String AlarmStatus_Contract_HumiditySensorsView = "AlarmHumiditySensorsViewContract";

            public static Uri AlarmUri_Status_Inputs_View = new Uri(AlarmStatus_Contract_InputsView, UriKind.Relative);
            public static Uri AlarmUri_Status_Outputs_View = new Uri(AlarmStatus_Contract_OutputsView, UriKind.Relative);
            public static Uri AlarmUri_Status_Relays_View = new Uri(AlarmStatus_Contract_RelaysView, UriKind.Relative);
            public static Uri AlarmUri_Status_Zones_View = new Uri(AlarmStatus_Contract_ZonesView, UriKind.Relative);
            public static Uri AlarmUri_Status_Sensors_View = new Uri(AlarmStatus_Contract_SensorsView, UriKind.Relative);
            public static Uri AlarmUri_Status_Device_View = new Uri(AlarmStatus_Contract_DeviceView, UriKind.Relative);
            public static Uri AlarmUri_Status_Events_View = new Uri(AlarmStatus_Contract_EventsView, UriKind.Relative);
            public static Uri AlarmUri_Status_Power_View = new Uri(AlarmStatus_Contract_PowerView, UriKind.Relative);
            public static Uri AlarmUri_Status_Network_View = new Uri(AlarmStatus_Contract_NetworkView, UriKind.Relative);


        #endregion



        #region AlarmSystemEntitiesConfig


        public const String AlarmConfig_Contract_AuthConfigView = "AuthConfigViewContract";
        public const String AlarmConfig_Contract_GlobalConfigView = "GlobalConfigViewContract";
        public const String AlarmConfig_Contract_CommConfigView = "CommConfigViewContract";
        public const String AlarmConfig_Contract_GsmConfigView = "GsmConfigViewContract";
        public const String AlarmConfig_Contract_GsmRcptConfigView = "GsmRcptConfigViewContract";
        public const String AlarmConfig_Contract_ZoneConfigView = "ZoneConfigViewContract";
        public const String AlarmConfig_Contract_ZoneMapConfigView = "ZoneMapConfigViewContract";
        public const String AlarmConfig_Contract_DeviceMapConfigView = "DeviceMapConfigViewContract";
        public const String AlarmConfig_Contract_UsersConfigView = "UserConfigViewContract";
        public const String AlarmConfig_Contract_SystemUsersConfigView = "SystemUserConfigViewContract";
        public const String AlarmConfig_Contract_Input_Config_View = "AlarmInputConfigureView";
        public const String AlarmConfig_Contract_Input_Config_View_Key_Name = "InputUuid";
        public const String AlarmConfig_Contract_Zone_Config_View_Key_Name = "ZoneId";

        public const String AlarmConfig_Contract_Output_Config_View = "AlarmOutputConfigureViewContract";
            public const String AlarmConfig_Contract_Relay_Config_View = "AlarmRelayConfigureViewContract";
            public const String AlarmConfig_Contract_Zone_Config_View = "AlarmZoneConfigureViewContract";
            public const String AlarmConfig_Contract_Sensor_Config_View = "AlarmSensorConfigureViewContract";
            public const String AlarmConfig_Contract_Device_Config_View = "AlarmDeviceConfigureViewContract";
        

        public static Uri AlarmUri_Config_Input_View = new Uri(AlarmConfig_Contract_Input_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Output_View = new Uri(AlarmConfig_Contract_Output_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Relay_View = new Uri(AlarmConfig_Contract_Relay_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Zone_View = new Uri(AlarmConfig_Contract_Zone_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Sensor_View = new Uri(AlarmConfig_Contract_Sensor_Config_View, UriKind.Relative);
            public static Uri AlarmUri_Config_Device_View = new Uri(AlarmConfig_Contract_Device_Config_View, UriKind.Relative);

        #endregion



        #region AlarmDevicesViews


            public const String AlarmStatus_Contract_Device_InputsModule_View = "AlarmDeviceInputsModuleViewContract";
            public const String AlarmStatus_Contract_Device_Keypad_View = "AlarmDeviceKeypadViewContract";
            public const String AlarmStatus_Contract_Device_Motherboard_View = "AlarmDeviceMotherboardViewContract";
            public const String AlarmStatus_Contract_Device_Sensor_View = "AlarmDeviceSensorViewContract";
            public const String AlarmStatus_Contract_Device_RelayModule_View = "AlarmSensorStatusureViewContract";
            public const String AlarmStatus_Contract_Device_Siren_View = "AlarmDeviceSirenViewContract";

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

            public const String AlarmStatus_Contract_Device_List_View = "AlarmSystemDeviceListViewContract";
            public static Uri AlarmUri_Status_Device_List_View = new Uri(AlarmStatus_Contract_Device_List_View, UriKind.Relative);

        public const String AlarmStatus_Contract_Global_View = "AlarmSystemGlobalViewContract";
        public static Uri AlarmUri_Status_Device_Global_View = new Uri(AlarmStatus_Contract_Global_View, UriKind.Relative);

        public const String AlarmStatus_Contract_Connection_Status_View = "AlarmSsstemConnectivityStatusViewContract";
        public static Uri AlarmUri_Contract_Connection_Status_View = new Uri(AlarmStatus_Contract_Connection_Status_View, UriKind.Relative);

        public const String AlarmMap_Contract_Zone_Context_Edit_View = "AlarmSytemZoneEditContextView";
        public const String AlarmMap_Contract_Device_Context_Edit_View = "AlarmSytemDeviceEditContextView";

        #endregion


    }


    public static class SiteManagmentRegionNames
    {
        public const String MainContentRegion = "SiteWizardContentRegion";
        public const String MainNavigationRegion = "SiteWizardNavigationRegion";
        

        public const String SiteConnectionWizard_Contract_ManualEntry_View = "SiteConnectionWizardManualEntryView";
        public static Uri SiteConnectionWizard_Uri_ManualEntry_View = new Uri(SiteConnectionWizard_Contract_ManualEntry_View, UriKind.Relative);

        public const String SiteConnectionWizard_Contract_MethodSelection_View = "SiteConnectionWizardMethodSelectionView";
        public static Uri SiteConnectionWizard_Uri_MethodSelection_View = new Uri(SiteConnectionWizard_Contract_MethodSelection_View, UriKind.Relative);

        public const String SiteConnectionWizard_Contract_SearchSitesList_View = "SiteConnectionWizardSearchSitesListView";
        public static Uri SiteConnectionWizard_Uri_SearchSitesList_View = new Uri(SiteConnectionWizard_Contract_SearchSitesList_View, UriKind.Relative);

        public const String SiteConnectionWizard_Contract_Summary_View = "SiteConnectionWizardSummaryView";
        public static Uri SiteConnectionWizard_Uri_Summary_View = new Uri(SiteConnectionWizard_Contract_Summary_View, UriKind.Relative);

        public const String SiteConnectionWizard_Contract_Test_View = "SiteConnectionWizardTestView";
        public static Uri SiteConnectionWizard_Uri_Test_View = new Uri(SiteConnectionWizard_Contract_Test_View, UriKind.Relative);

        public const String SiteConnectionWizard_Contract_UsbList_View = "SiteConnectionWizardUsbListView";
        public static Uri SiteConnectionWizard_Uri_UsbList_View = new Uri(SiteConnectionWizard_Contract_UsbList_View, UriKind.Relative);
        
    }


    public static class GlobalViewContractNames
    {
        public const String Global_Contract_Menu_RightSide_AlarmMapContext = "GlobalContractMenuRightSideAlarmMapContext";
        public const String Global_Contract_Menu_RightSide_AlarmZoneEditMapContext = "GlobalContractMenuRightSideZoneEditMapContext";
        public const String Global_Contract_Menu_RightSide_AlarmDeviceEditMapContext = "GlobalContractMenuRightSideDeviceEditMapContext";
        public const String Global_Contract_Menu_RightSide_AlarmZoneEditListItemContext = "GlobalContractMenuRightSideZoneEditListItemContext";
        public const String Global_Contract_Menu_RightSide_AlarmAuthorizedDeviceEditListItemContext = "GlobalContractMenuRightSideAuthorizedDeviceEditListItemContext";
        public const String Global_Contract_Menu_RightSide_AlarmRemoteUserEditListItemContext = "GlobalContractMenuRightSideRemoteUserEditListItemContext";
        public const String Global_Contract_Menu_RightSide_AlarmSystemUserEditListItemContext = "GlobalContractMenuRightSideSystemUserEditListItemContext";
        public const String Global_Contract_Menu_RightSide_AlarmGsmRcptEditListItemContext = "GlobalContractMenuRightSideGsmRcptEditListItemContext";

        public const String Global_Contract_Menu_RightSide_Grid_Nav = "GlobalContractMenuRightSideGridNav";

        public const String Global_Contract_Menu_Top_AlarmSystemContext = "GlobalContractMenuTopAlarmSystemContext";
        public const String Global_Contract_Menu_Top_CctvContext = "GlobalContractMenuTopCctvContext";

        public const String Global_Contract_Footer_ConnectivityModeContext= "GlobalContractFooterConnectivityModeContext";

        public const String Global_Contract_Menu_Left_SiteList = "GlobalContractMenuLeftSiteList";

   
        public const String Global_Contract_Nav_Site_Context__Key_Name = "SiteUUID";

    }

    public static class AlarmSystemMapContractNames
    {
        public const String Alarm_Contract_Map_Zone_Edit_Context_Key_Name = "ZoneUUID";
        public const String Alarm_Contract_Map_Device_Edit_Context_Key_Name = "DeviceUUID";
    }

    public static class AlarmSystemEntityListContractNames
    {
        public const String Alarm_Contract_Entity_Zone_Edit_Context_Key_Name = "ZoneUUID";
        public const String Alarm_Contract_Entity_AuthorizedDevice_Edit_Context_Key_Name = "DeviceUUID";
        public const String Alarm_Contract_Entity_SystemUser_Edit_Context_Key_Name = "UserUUID";
        public const String Alarm_Contract_Entity_RemoteUser_Edit_Context_Key_Name = "RemoteUserUUID";
        public const String Alarm_Contract_Entity_GsmRcpt_Edit_Context_Key_Name = "GsmRcptUUID";

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
        public const String FooterLeftNavigationRegion = "FooterLeftNavigationRegion";
        public const String TopContextToolbarRegion = "TopContextToolbarRegion";
    }
    
}
