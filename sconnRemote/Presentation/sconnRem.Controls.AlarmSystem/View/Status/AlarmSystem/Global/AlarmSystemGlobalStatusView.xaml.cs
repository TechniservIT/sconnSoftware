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
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm;
using sconnRem.Navigation;
using sconnRem.ViewModel.Alarm;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Global
{

    [Export(AlarmRegionNames.AlarmStatus_Contract_Global_View)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmSystemGlobalStatusView : UserControl
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        [Import]
        public AlarmGlobalConfigViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        public AlarmSystemGlobalStatusView()
        {
            InitializeComponent();
        }
    }
}
