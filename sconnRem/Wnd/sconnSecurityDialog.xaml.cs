using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;    
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using sconnRem.Properties;


namespace sconnRem
{
    /// <summary>
    /// Interaction logic for sconnSecurityDialog.xaml
    /// </summary>
    /// 
    public partial class sconnSecurityDialog : Window
    {

        private bool _valid = false;
        public bool UserValidated { get { return _valid; } set { _valid = value; } }

        public sconnSecurityDialog()
        {
            
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            InitializeComponent();
            lblUsernameDesc.Content = Properties.Resources.ResourceManager.GetString("lblUsernameDesc");
            lblPasswordDesc.Content = Properties.Resources.ResourceManager.GetString("lblPasswordDesc");
            loginButton.Content = Properties.Resources.ResourceManager.GetString("btnLoginDesc");
            this.Title = Properties.Resources.ResourceManager.GetString("wndSecurityDialogTitle");
            this.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



    }
}
