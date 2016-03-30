using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using sconnConnector;
using sconnConnector.POCO.Config;

namespace sconnRem
{
    /// <summary>
    /// Interaction logic for changePasswordWnd.xaml
    /// </summary>
    {

        {
            InitializeComponent();
        }

        {
        }

        private void savePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            {
                for (int i = 0; i < passwdFormated.GetLength(0); i++)
                {
                    site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrSitePasswd] = passwdFormated[i];
                }
                //clear unused password bytes
                int passwdLen = passwdFormated.GetLength(0);
                for (int j = 0; j < ipcDefines.PasswordSize -passwdLen ; j++)
                {
                    site.siteCfg.globalConfig.memCFG[ipcDefines.mAdrSitePasswd + passwdLen + j] = 0;
                }
                {
                    this.Close(); //close window after success
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Password mismatch");
            }

        }


    }
}
