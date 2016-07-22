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
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm.EntityList;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.View.Config.AlarmSystem.Context
{
    /// <summary>
    /// Interaction logic for AlarmEntityRemoteAccountEditContext.xaml
    /// </summary>

    [Export(GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmRemoteUserEditListItemContext)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlarmEntityRemoteAccountListItemEditContext : UserControl
    {
        [ImportingConstructor]
        public AlarmEntityRemoteAccountListItemEditContext(AlarmEntityRemoteUserEditContextViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }

}
