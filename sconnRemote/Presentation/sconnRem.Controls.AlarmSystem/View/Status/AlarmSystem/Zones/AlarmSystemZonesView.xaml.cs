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
using sconnRem.Navigation;
using sconnRem.ViewModel.Alarm;

namespace sconnRem.Controls.AlarmSystem.View.Status.Zones
{
    [Export(AlarmRegionNames.AlarmConfig_Contract_ZoneConfigView)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmSystemZonesView : UserControl
    {
        [ImportingConstructor]
        public AlarmSystemZonesView(AlarmZoneConfigViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }


    }
}
