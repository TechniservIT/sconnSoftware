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

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Events
{

    [Export(AlarmRegionNames.AlarmStatus_Contract_EventsView)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmEventsListView : UserControl
    {
        private const string MainContentRegionName = GlobalViewRegionNames.MainGridContentRegion;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        private static Uri configureUri = new Uri(AlarmRegionNames.AlarmStatus_Contract_EventsView,
            UriKind.Relative);

        [Import] public IRegionManager RegionManager;

        [ImportingConstructor]
        public AlarmEventsListView(AlarmEventsListViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        
    }

}
