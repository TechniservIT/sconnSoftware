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
using System.Windows.Navigation;
using System.Windows.Shapes;
using sconnConnector.Config;
using sconnRem.ViewModel.Generic;

namespace sconnRem.View.Config
{
    /// <summary>
    /// Interaction logic for ConfigSelect.xaml
    /// </summary>
    public partial class ConfigSelect : UserControl
    {
        private AlarmSystemConfigManager _manager;

        public ConfigSelect()
        {
            InitializeComponent();
        }

        public ConfigSelect(AlarmSystemConfigManager manager)
        {
            this._manager = manager;
            InitializeComponent();
        }

        public void DidSelectItem()
        {

        }

        private void Show_GlobalConfig(object sender, RoutedEventArgs e)
        {

        }

        private void Show_Settings(object sender, RoutedEventArgs e)
        {

        }

     

    }
}
