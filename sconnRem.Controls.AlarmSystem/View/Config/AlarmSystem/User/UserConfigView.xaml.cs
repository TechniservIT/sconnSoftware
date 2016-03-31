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
using sconnRem.ViewModel.Alarm;

namespace sconnRem.View.Config
{
    /// <summary>
    /// Interaction logic for UserConfigView.xaml
    /// </summary>


    [Export("UserConfigView")]
    public partial class UserConfigView : UserControl
    {
        [ImportingConstructor]
        public UserConfigView(AlarmUsersConfigViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
    
}
