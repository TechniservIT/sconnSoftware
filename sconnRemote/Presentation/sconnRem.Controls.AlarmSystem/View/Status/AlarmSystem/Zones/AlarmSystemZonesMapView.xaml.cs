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
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm;
using sconnRem.Navigation;
using sconnRem.ViewModel.Alarm;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Zones
{

    [Export(AlarmRegionNames.AlarmConfig_Contract_ZoneMapConfigView)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmSystemZonesMapView : UserControl
    {
        [ImportingConstructor]
        public AlarmSystemZonesMapView(AlarmZoneMapViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
         
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            string id = (string)((StackPanel)sender).Tag;
            AlarmZoneMapViewModel model = (AlarmZoneMapViewModel) DataContext;
            model?.VertexWithIdSelected(id);
        }
    }
}
