using sconnConnector.Config;
using System;
using System.Collections.Generic;
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
using System.ComponentModel.Composition;
using Prism.Regions;
using Prism.Modularity;

namespace sconnRem.Wnd.Config
{
    /// <summary>
    /// Interaction logic for wndConfigureSite.xaml
    /// </summary>
    /// 


    public static class RegionNames
    {
        public const String MainContentRegion = "MainContentRegion";
        public const String MainNavigationRegion = "MainNavigationRegion";
    }


    [Export]
    public partial class wndConfigureSiteShell : Window, IPartImportsSatisfiedNotification
    {

        private const string StartModuleName = "AlarmAuthConfigModule";
        private static Uri StartViewUri = new Uri("/View/Config/AlarmSystem/AuthConfig", UriKind.Relative);

        //private const string EmailModuleName = "EmailModule";
        //private static Uri InboxViewUri = new Uri("/InboxView", UriKind.Relative);


        public wndConfigureSiteShell()
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
                            RegionNames.MainContentRegion,
                            StartViewUri);
                    }
                };
        }


    }


}
