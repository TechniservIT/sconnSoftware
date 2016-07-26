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
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Zones
{
    [Export(AlarmRegionNames.AlarmConfig_Contract_ZoneConfigView)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmSystemZoneConfigureView : UserControl
    {

        [ImportingConstructor]
        public AlarmSystemZoneConfigureView(AlarmMapZoneEditContextViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
