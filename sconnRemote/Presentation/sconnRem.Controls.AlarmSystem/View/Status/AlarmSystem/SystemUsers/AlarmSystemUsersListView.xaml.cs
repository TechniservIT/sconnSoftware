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

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.SystemUsers
{
    /// <summary>
    /// Interaction logic for AlarmSystemUsersListView.xaml
    /// </summary>

    [Export(AlarmRegionNames.AlarmStatus_Contract_SystemUsers)]
    [ViewSortHint("01")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmSystemUsersListView : UserControl
    {
        private const string MainContentRegionName = GlobalViewRegionNames.MainGridContentRegion;
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        [Import]
        public IRegionManager RegionManager;

        [ImportingConstructor]
        public AlarmSystemUsersListView(AlarmSystemUsersConfigViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

    }

}
