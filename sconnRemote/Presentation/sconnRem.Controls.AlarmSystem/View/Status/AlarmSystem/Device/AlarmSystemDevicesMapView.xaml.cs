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
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Device
{
    /// <summary>
    /// Interaction logic for AlarmSystemDevicesMapView.xaml
    /// </summary>


    [Export(AlarmRegionNames.AlarmConfig_Contract_DeviceMapConfigView)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmSystemDevicesMapView : UserControl
    {
        [ImportingConstructor]
        public AlarmSystemDevicesMapView(AlarmDeviceMapViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
        

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            string id = (string)((StackPanel)sender).Tag;
            AlarmDeviceMapViewModel model = (AlarmDeviceMapViewModel)DataContext;
            model?.VertexWithIdSelected(id);
        }
    }


}
