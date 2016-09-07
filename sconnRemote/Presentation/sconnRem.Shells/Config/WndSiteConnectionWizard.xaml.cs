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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Mef.Regions;
using Prism.Modularity;
using Prism.Regions;
using sconnRem.Controls.SiteManagment.Wizard;
using sconnRem.Navigation;

namespace sconnRem.Shells.Config
{

    [Export]
    public partial class WndSiteConnectionWizard : Window, IPartImportsSatisfiedNotification
    {
        [Import]
        public IRegionManager RegionManager;

        [ImportingConstructor]
        public WndSiteConnectionWizard(SiteConnectionWizardViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.Loaded += WndSiteConnectionWizard_Loaded;
            InitializeComponent();
        }

        private void WndSiteConnectionWizard_Loaded(object sender, RoutedEventArgs e)
        {
            this.RegionManager.RequestNavigate(SiteManagmentRegionNames.MainContentRegion,
               (((SiteConnectionWizardViewModel)this.DataContext).Config.serverIP != null
                 ? SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View
                 : SiteManagmentRegionNames.SiteConnectionWizard_Contract_SearchSitesList_View));

        }

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;
        

        public void OnImportsSatisfied()
        {
          
        }

    }

}
