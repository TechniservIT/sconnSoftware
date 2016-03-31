using Prism.Regions;
using sconnRem.ViewModel.Alarm;
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

namespace sconnRem.View.Config
{
    /// <summary>
    /// Interaction logic for CommConfigView.xaml
    /// </summary>
    /// 

    [Export("CommConfigView")]
    public partial class CommConfigView : UserControl
    {
        //[Import]
        //public AlarmCommConfigViewModel ViewModel
        //{
        //    set { this.DataContext = value; }
        //}


        [ImportingConstructor]
        public CommConfigView(AlarmCommConfigViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }


        //public CommConfigView()
        //{
        //    InitializeComponent();
        //}

       
    }

}
