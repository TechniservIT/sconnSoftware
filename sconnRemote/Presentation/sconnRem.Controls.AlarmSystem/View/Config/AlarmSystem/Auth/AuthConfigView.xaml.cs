using sconnRem.ViewModel.Alarm;
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
using sconnRem.Navigation;

namespace sconnRem.View.Config
{
    /// <summary>
    /// Interaction logic for AuthConfig.xaml
    /// </summary>
    /// 

 //   [Export(AlarmRegionNames.AlarmConfig_Contract_AuthConfigView)]
    public partial class AuthConfigView : UserControl
    {
        [ImportingConstructor]
        public AuthConfigView(AlarmAuthConfigViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }

}
