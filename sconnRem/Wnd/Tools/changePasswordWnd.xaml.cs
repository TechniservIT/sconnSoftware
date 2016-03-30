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
    public partial class ChangePasswordWnd : Window
    {
        private int _siteId;
        private sconnCfgMngr _cfgMan;
        private sconnDataSrc _configSource;

        public ChangePasswordWnd(int site)
        {
            InitializeComponent();
            _siteId = site;
            InputPasswordTb.MaxLength = ipcDefines.PasswordMaxChars;
            InputConfirmTb.MaxLength = ipcDefines.PasswordMaxChars;
            _cfgMan = new sconnCfgMngr();
            _configSource = new sconnDataSrc();
        }

        private bool InputDataValid()
        {
            return InputConfirmTb.Text == InputPasswordTb.Text ? true : false;
        }

        private void savePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InputDataValid())
            {
                sconnSite site = sconnDataShare.getSite(_siteId);
                byte[] passwdFormated = Encoding.UTF8.GetBytes(InputConfirmTb.Text.ToCharArray());
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
                if (_cfgMan.WriteGlobalCfg( site))//upload changed password
                {
                    site.authPasswd = InputConfirmTb.Text; //update runtime password for auth
                    _configSource.SaveConfig(DataSourceType.xml); // save changes to file        
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
