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
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map;

namespace sconnRem.Controls.AlarmSystem.View.Config.AlarmSystem.Context
{
    /// <summary>
    /// Interaction logic for AlarmMapEditContextView.xaml
    /// </summary>

    [Export]
    [ViewSortHint("01")]
    public partial class AlarmMapEditContextView : UserControl
    {
        [ImportingConstructor]
        public AlarmMapEditContextView(AlarmMapEntityEditContextViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
