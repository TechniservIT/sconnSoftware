using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Prism.Modularity;
using Prism.Regions;

namespace sconnRem.Wnd.Main
{
    /// <summary>
    /// Interaction logic for wndGlobalShell.xaml
    /// </summary>

    public static class RegionNames
    {
        public const String MainContentRegion = "MainViewGridRegion";
        public const String RNavigationRegion = "RightSideToolbarRegion";
        public const String LNavigationRegion = "LeftSideMenuRegion";
        public const String RopNavigationRegion = "TopToolbarRegion";
    }


    [Export]
    {

        private const string StartModuleName = "AlarmAuthConfigModule";

        //private const string EmailModuleName = "EmailModule";
        //private static Uri InboxViewUri = new Uri("/InboxView", UriKind.Relative);


        {
            InitializeComponent();
        }

        private void Show_GlobalConfig(object sender, RoutedEventArgs e)
        {

        }

        private void Show_Settings(object sender, RoutedEventArgs e)
        {


        }
        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        [Import(AllowRecomposition = false)]
        public IRegionManager RegionManager;

        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted +=
                (s, e) =>
                {

                    if (e.ModuleInfo.ModuleName == StartModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                            Config.RegionNames.MainContentRegion,
                    }
                };
        }


    }

}
