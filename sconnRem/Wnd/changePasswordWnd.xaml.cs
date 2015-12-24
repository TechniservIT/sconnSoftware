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

namespace sconnRem
{
    /// <summary>
    /// Interaction logic for changePasswordWnd.xaml
    /// </summary>
    public partial class changePasswordWnd : Window
    {
        private int _siteID;
        private sconnCfgMngr cfgMan;
        private sconnDataSrc configSource;

        public changePasswordWnd(int site)
        {
            InitializeComponent();
            _siteID = site;
            inputPasswordTB.MaxLength = ipcDefines.PasswordMaxChars;
            inputConfirmTB.MaxLength = ipcDefines.PasswordMaxChars;
            cfgMan = new sconnCfgMngr();
            configSource = new sconnDataSrc();
        }

        private bool inputDataValid()
        {
            return inputConfirmTB.Text == inputPasswordTB.Text ? true : false;
        }

        private void savePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (inputDataValid())
            {
                sconnSite site = sconnDataShare.getSite(_siteID);
                byte[] passwdFormated = Encoding.UTF8.GetBytes(inputConfirmTB.Text.ToCharArray());
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
                if (cfgMan.WriteGlobalCfg( site))//upload changed password
                {
                    site.authPasswd = inputConfirmTB.Text; //update runtime password for auth
                    configSource.SaveConfig(DataSourceType.xml); // save changes to file        
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
