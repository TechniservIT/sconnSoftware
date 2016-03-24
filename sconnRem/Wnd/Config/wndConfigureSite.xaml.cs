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

namespace sconnRem.Wnd.Config
{
    /// <summary>
    /// Interaction logic for wndConfigureSite.xaml
    /// </summary>
    /// 


    [Export]
    public partial class wndConfigureSiteShell : Window, IPartImportsSatisfiedNotification
    {

        private const string EmailModuleName = "EmailModule";
        private static Uri InboxViewUri = new Uri("/InboxView", UriKind.Relative);

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
                    // todo: 01 - Navigation on when modules are loaded.
                    // When using region navigation, be sure to use it consistently
                    // to ensure you get proper journal behavior.  If we mixed
                    // usage of adding views directly to regions, such as through
                    // RegionManager.AddToRegion, and then use RegionManager.RequestNavigate,
                    // we may not be able to navigate back correctly.
                    // 
                    // Here, we wait until the module we want to start with is
                    // loaded and then navigate to the view we want to display
                    // initially.
                    //     
                    if (e.ModuleInfo.ModuleName == EmailModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                            RegionNames.MainContentRegion,
                            InboxViewUri);
                    }
                };
        }


    }
}
