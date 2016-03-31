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
    public partial class WndConfigureSiteShell : Window, IPartImportsSatisfiedNotification
    {

        private const string StartModuleName = "AlarmAuthConfigModule";
        private static Uri _startViewUri = new Uri("/View/Config/AlarmSystem/AuthConfig", UriKind.Relative);
        
        public WndConfigureSiteShell()
        {
            InitializeComponent();
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
                            _startViewUri);
                    }
                };
        }


    }


}
