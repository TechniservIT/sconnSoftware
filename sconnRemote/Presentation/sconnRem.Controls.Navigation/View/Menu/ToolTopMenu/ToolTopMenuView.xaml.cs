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

namespace sconnRem.View.Menu.ToolTopMenu
{
    [Export]
    [ViewSortHint("01")]
    public partial class ToolTopMenuView : UserControl
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static Uri _TargetNavUri = new Uri("AuthConfigView", UriKind.Relative);

        [Import]
        public IRegionManager RegionManager;


        [ImportingConstructor]
        public ToolTopMenuView(ToolTopMenuViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        
    }

}
