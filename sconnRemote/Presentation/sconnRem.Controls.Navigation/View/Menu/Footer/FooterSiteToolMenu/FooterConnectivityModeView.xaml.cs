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

namespace sconnRem.Controls.Navigation.View.Menu.Footer.FooterSiteToolMenu
{
    /// <summary>
    /// Interaction logic for FooterConnectivityModeView.xaml
    /// </summary>

    [Export]
    public partial class FooterConnectivityModeView : UserControl
    {
        private readonly Logger _nlogger = LogManager.GetCurrentClassLogger();

        [ImportingConstructor]
        public FooterConnectivityModeView(ConnectivityModeFooterViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }


    }


}
