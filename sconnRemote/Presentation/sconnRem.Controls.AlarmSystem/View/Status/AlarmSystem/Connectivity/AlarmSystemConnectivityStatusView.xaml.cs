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

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Connectivity
{

    [Export(AlarmRegionNames.AlarmStatus_Contract_Connection_Status_View)]
    [ViewSortHint("01")]
    public partial class AlarmSystemConnectivityStatusView : UserControl
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        [Import]
        public IRegionManager RegionManager;

        [ImportingConstructor]
        public AlarmSystemConnectivityStatusView()
        {
            InitializeComponent();
        }
        

    }



}
