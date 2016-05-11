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
using AlarmSystemManagmentService;
using NLog;
using Prism.Regions;
using sconnRem.Navigation;
using sconnRem.ViewModel.Alarm;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Users
{

    [Export(AlarmRegionNames.AlarmConfig_Contract_UsersConfigView)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AlarmUsersListView : UserControl
    {
        private const string MainContentRegionName = GlobalViewRegionNames.MainGridContentRegion;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        private static Uri configureUri = new Uri(AlarmRegionNames.AlarmStatus_Contract_EventsView,
            UriKind.Relative);

        [Import]
        public IRegionManager RegionManager;

        [ImportingConstructor]
        public AlarmUsersListView(AlarmUsersConfigViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

    }


}
