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

        public static Uri AlarmUri_Config_Auth_View = new Uri("AuthConfigView", UriKind.Relative);
        public static Uri AlarmUri_Config_Global_View = new Uri("GlobalConfigView", UriKind.Relative);
        public static Uri AlarmUri_Config_Gsm_View = new Uri("GsmConfigView", UriKind.Relative);
        public static Uri AlarmUri_Config_Zone_View = new Uri("ZoneConfigView", UriKind.Relative);
        public static Uri AlarmUri_Config_Users_View = new Uri("UserConfigView", UriKind.Relative);


    }
    
    public static class GlobalViewRegionNames
    {
        public const String MainGridContentRegion = "MainViewGridRegion";
        public const String RNavigationRegion = "RightSideToolbarRegion";
        public const String LNavigationRegion = "LeftSideMenuRegion";
        public const String RopNavigationRegion = "TopToolbarRegion";
    }
    
}
