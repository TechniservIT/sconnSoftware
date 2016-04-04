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
using Prism.Mef.Regions;
using Prism.Regions;
using Prism.Modularity;
using sconnRem.Navigation;

namespace sconnRem.Wnd.Config
{

    [Export]
    public partial class WndConfigureSiteShell : Window, IPartImportsSatisfiedNotification
    {

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        //[Import(AllowRecomposition = false)]
        //public IRegionManager RegionManager;

        public IRegionManager RegionManager { get; private set; }


        private const string StartModuleName = "AlarmAuthConfigModule";
        private static Uri _startViewUri = new Uri("/View/Config/AlarmSystem/AuthConfig", UriKind.Relative);
        
        public WndConfigureSiteShell()
        {
            RegionManager = new MefRegionManager();
            InitializeComponent();
        }
        


        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted +=
                (s, e) =>
                {

                    if (e.ModuleInfo.ModuleName == StartModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                            AlarmRegionNames.MainContentRegion,
                            _startViewUri);
                    }
                };
        }


    }


}
