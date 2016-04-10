using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
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
using Prism.Mef.Modularity;
using Prism.Mef.Regions;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using sconnRem.Model.Global;
using sconnRem.Navigation;

namespace sconnRem.Wnd.Main
{
    /// <summary>
    /// Interaction logic for wndGlobalShell.xaml
    /// </summary>




    [Export]
    public partial class WndGlobalShell : Window    //, IPartImportsSatisfiedNotification
    {

        private const string StartModuleName = "AlarmAuthConfigModule";
        private static Uri _startViewUri = new Uri("/View/Config/AlarmSystem/AuthConfig", UriKind.Relative);


        public WndGlobalShell(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
            InitializeComponent();
        }

        public IRegionManager RegionManager { get; private set; }


        public WndGlobalShell()
        {
            RegionManager= new MefRegionManager();
            InitializeComponent();
        }

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        //[Import(AllowRecomposition = false)]
        //public IRegionManager RegionManager;

        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted +=
                (s, e) =>
                {

                    if (e.ModuleInfo.ModuleName == StartModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                           GlobalViewRegionNames.MainGridContentRegion,
                            _startViewUri);
                    }
                };
        }
        

    }

}
