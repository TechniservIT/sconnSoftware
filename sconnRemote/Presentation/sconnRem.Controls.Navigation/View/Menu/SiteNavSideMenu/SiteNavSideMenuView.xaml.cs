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
using NLog;
using Prism.Regions;
using sconnRem.Controls.Navigation.ViewModel.Navigation;
using sconnRem.Navigation;

namespace sconnRem.View.Menu.SiteNavSideMenu
{
    
    [Export]
    [ViewSortHint("01")]
    public partial class SiteNavSideMenuView : UserControl
    {
        [Import]
        public IRegionManager RegionManager;
        
        [ImportingConstructor]
        public SiteNavSideMenuView(SiteNavViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        
       
    }


}
