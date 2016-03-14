using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using sconnRem.Properties;
using sconnConnector;
using sconnConnector.POCO.Config;

namespace sconnRem
{

    public  partial class sconnView : Window
    {
        
        private static System.Timers.Timer sconnTask;
        //private static StackPanel dataViewPanel;
        private static double _viewHeight;
        private static double _viewWidth;
        private int ViewedSiteId = 0;
        private sconnDataSrc ConfigSource  = new sconnDataSrc();
        private sconnSecurityDialog seDiag;
        private sconnSiteManagerWindow siteManagerWnd;
        private statusViewPanel statusView;
        private SitePanel sitesPanel;
        private SiteView siteView;
        private SiteEditView siteEditView;
        private MapWindow mapView;
        private sconnCfgMngr ConfigManager = new sconnCfgMngr();
        private GlobalSettings settings = new GlobalSettings();

        public static double viewHeight { get { return _viewHeight; } set { _viewHeight = value; } }
        public static double viewWidth { get { return _viewWidth; } set { _viewWidth = value; } }

        private delegate void updateSiteViewDelegate(); //site status update del function

        public void populateDataView(StackPanel panel)
        {
            dataView.Children.Clear();
            dataView.Children.Add(panel);
        }

        private void Bootstrap_FindAndLoad_UsbDevices()
        {
            USB usbcomm = new USB();
             //   usbcomm.UsbComm_Test_Trx();
            //  usbcomm.UsbComm_Sample_Trx();
            bool UsbConn = usbcomm.TestConnection();
            if (UsbConn)
            {
                sconnSite site = new sconnSite("USB_PROG",400,"",0,"");   //string siteName, int intervalMs, string server, int port, string password)
                site.UsbCom = true;
                sconnDataShare.addSite(site);
                ConfigManager.saveConfig();
            }


            // string resp = usbcomm.ReadUsbBlocking();
            // usbcomm.TransmitLoop();
        }

        public sconnView()
        {
            
            ConfigSource.SetXmlPath(Directory.GetCurrentDirectory().ToString());
            ConfigSource.SetXmlFileName("sconnRem.xml");
            settings = ConfigSource.LoadRegistryData();

            CultureInfo info = new CultureInfo(settings.CultureName);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = info; //CultureInfo.InstalledUICulture;
            Thread.CurrentThread.CurrentUICulture = info;
            Thread.CurrentThread.CurrentCulture = info;

            //prompt for access before init
            seDiag = new sconnSecurityDialog();
            seDiag.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
           // seDiag.Topmost = true;
            seDiag.Closed += seDiag_Closed;
            seDiag.Closing += seDiag_Closing;
            seDiag.KeyDown += seDiag_KeyDown;
            seDiag.loginButton.Click += loginButton_Click;


            Bootstrap_FindAndLoad_UsbDevices();


            //mapView = new MapWindow();
            //mapView.Show();

            statusView = new statusViewPanel();

            initMainView();
            this.Hide(); //hide gui until login 


        }


        private void initMainView()
        {
            initSiteConfig();
            InitializeComponent();
            initSiteList();
           
            viewHeight = dataView.Height;
            viewWidth = dataView.Width;
        }
      

        private void initSiteConfig()
        {

            /**** site config is read from file no need to add */
            ConfigSource.LoadConfig(DataSourceType.xml);
            //sconnDataShare.removeSites();

            sconnTask = new System.Timers.Timer(1000);
            sconnTask.Elapsed += new ElapsedEventHandler(ProcessSiteUpdates);
            sconnTask.Interval = 1000;
           // sconnTask.Start();  -disable for network debug

        }


        void seDiag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RoutedEventArgs ne = new RoutedEventArgs();
                loginButton_Click(sender, ne);
            }
        }

        private void LoadSiteView()
        {
            if (sconnDataShare.SiteLiveViewEnabled == true)
            {
                int CurrentTabId = 0;//default is first tab;
                if (siteView != null)
                {
                    CurrentTabId = siteView.siteTabView.SelectedIndex;
                }
                siteView = new SiteView(ViewedSiteId);
                siteView.siteTabView.SelectedIndex = CurrentTabId;
                dataView.Children.Clear();
                dataView.Children.Add(siteView);
            }
        }



        private void LoadSiteEdit()
        {
            siteEditView = new SiteEditView(ViewedSiteId);
            dataView.Children.Clear();
            dataView.Children.Add(siteEditView);
        }

        private void ProcessSiteUpdates(object source, ElapsedEventArgs e)
        {
            try
            {
                sconnSite[] sites = sconnDataShare.getSites();
                for (int i = 0; i < sites.GetLength(0); i++)
			    {
                    int currentSite = i;
                    if (sites[currentSite] != null)
                    {
                        if ((DateTime.Now - sites[currentSite].LastUpdate).TotalMilliseconds > sites[currentSite].statusCheckInterval)
                        {
                            sites[currentSite].LastUpdate = DateTime.Now;
                           // ThreadStart threadDelegate = new ThreadStart(updateSite);
                            Thread updateThread = new Thread(() => updateSite( sites[currentSite]));         
                            updateThread.Start();
                        }

                    }
                } 
                //run gui update delegate after sites are refereshed
                dataView.Dispatcher.Invoke(new updateSiteViewDelegate(LoadSiteView));

            }
            catch (Exception)
            {        
                throw;
            }
        }


        private void updateSite( sconnSite site)
        {
            ConfigManager.updateSiteConfig( site);  //update status for each device ( read IO registers )
        }

        private void updateSites()
        {
            sconnSite[] sites = sconnDataShare.getSites();
            for (int i = 0; i < sites.GetLength(0); i++)
			    {
                    ConfigManager.updateSiteConfig( sites[i]); //update status for each device ( read IO registers )
			    }
        }

        private bool isUserValid(string username, string password)
        {
            bool valid = false;
            //check sql/file user database
            if (username == "domo" && password == "domo")
            {
                return true;
            }
            return valid;
        }


        void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = seDiag.usernameInput.Text;
            string password = seDiag.passwordInput.Text;
            if (login != null && password != null)
            {
                if (isUserValid(login, password))
                {
                    //TODO : check user permissions
                    seDiag.UserValidated = true;
                    this.Show();
                    seDiag.Close();
                }
            }
        }


        void seDiag_Closing(object sender, EventArgs e)
        {
            if (!seDiag.UserValidated)
            {
                this.Close(); //close parent only if user did not auth
            }
        }

        void seDiag_Closed(object sender, EventArgs e)
        {
    
        }

        public  void setMainView(StackPanel viewContent)
        {
            dataView.Children.Clear();
            dataView.Children.Add(viewContent);
        }

        private void initSiteList()
        {
            siteList.Children.Clear();
            sconnSite[] sites = sconnDataShare.getSites();
            sitesPanel = new SitePanel(siteList.Width, siteList.Height); 
            foreach (sconnSite site in sites)
            {
                SitePanelItem item = new SitePanelItem(site.siteName, siteList.Width, siteList.Height *0.1, site.siteID);
                item.siteBtn1.Click += new RoutedEventHandler((sender, e) => ViewSiteClick(sender, e, item.siteName));
                item.siteBtn2.Click += new RoutedEventHandler((sender, e) => ConfigSiteClick(sender, e, item.siteName));
                //siteList.Children.Add(item);
                sitesPanel.addStatusItem(item);
            }
            sitesPanel.SelectItemChanged += sitesPanel_SelectItemChanged;        
            siteList.Children.Add(sitesPanel);
            ImageBrush img1 = new ImageBrush();
            ImageBrush img2 = new ImageBrush();
            ImageBrush img3 = new ImageBrush();
            img1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/cog3.jpg", UriKind.Absolute));
            btnSites.Background = img1;
            img2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/key1.jpg", UriKind.Absolute));
            btnConfig.Background = img2;
            img3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/stat1.jpg", UriKind.Absolute));
            btnMap.Background = img3;

        }

        void sitesPanel_SelectItemChanged(object sender, EventArgs e)
        {
            initSiteList();  
        }



        private void btnMap_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSites_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {

        }
  

        private void ConfSiteClick(object sender, RoutedEventArgs e)
        {

        }

        /********  connect and upload config to remote device in background ******/
        private void configureSiteThread(int siteID)
        {
            Thread updateThread = new Thread(() => configureSite(siteID));
            updateThread.Start();
        } 
    
        private void configureSite(int siteID)
        {
            //write config to remote device
            sconnSite site = sconnDataShare.getSite(siteID);

            ConfigManager.WriteGlobalCfg( site);
            ConfigManager.WriteDeviceCfg( site);
        }




        void SiteLoadingFinished(object sender, EventArgs e, bool success, int siteId)
        {
            if ( success == true)
            {
                sconnDataShare.SiteLiveViewEnabled = true;
                ViewedSiteId = siteId;
                LoadSiteView();

            }
        }

        private void ViewSiteClick(object sender, RoutedEventArgs e, string name) //object sender, RoutedEventArgs e, 
        {
            sconnDataShare.SiteLiveViewEnabled = true; //start live update
            string siteName = name;
            int index = 0;
            sconnSite[] sites = sconnDataShare.getSites();
            foreach (sconnSite sconnSite in sites)
            {
                if (sconnSite != null)
                {
                    if (sconnSite.siteName == siteName)
                    {
                        sconnDataShare.SiteLiveViewEnabled = false; //suppress view updates during connection test
                        ViewSiteLoading status = new ViewSiteLoading(sconnSite.siteID);
                        status.ConnectedDel += SiteLoadingFinished;
                        dataView.Children.Clear();
                        dataView.Children.Add(status);
                        status.TestConnection();
                      
                    }
                }
                index++;
            }

        }

        /******* site editor window ********/
        private void ConfigSiteClick(object sender, RoutedEventArgs e, string name)
        {
            sconnDataShare.SiteLiveViewEnabled = false; //disable live view during config
            string siteName = name;
            sconnSite[] sites = sconnDataShare.getSites();
            foreach (sconnSite sconnSite in sites)
            {
                if (sconnSite != null)
                {
                    if (sconnSite.siteName == siteName)
                    {
                        bool updated = true; //TODO: create non-blocking update
                        if (updated)
                        {
                            ViewedSiteId = sconnSite.siteID;
                            LoadSiteEdit();
                        }
                        else
                        {
                            statusView.setStatusText("Błąd połączenia");
                            dataView.Children.Clear();
                            dataView.Children.Add(statusView);
                        }
                    }
                }
            }

        }

        private void setupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            siteManagerWnd = new sconnSiteManagerWindow();
            siteManagerWnd.ConfigChanged += siteManagerWnd_ConfigChanged;
        }

        void siteManagerWnd_ConfigChanged(object sender, EventArgs e)
        {
            //reload sitelist when config was changed by config manager
            initSiteList();
        }

        private void saveConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConfigSource.SaveConfig(DataSourceType.xml);
        }

        private void LangSelectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
        

        private void fileMenuItem_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void File_SaveAs_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog Diag = new Microsoft.Win32.SaveFileDialog();
            Diag.FileName = "sconnRem_" + DateTime.Now.ToShortDateString();
            Diag.DefaultExt = ".xml";
            Diag.Filter = "Xml Document (*.xml)|*.xml|All files (*.*)|*.*";

            XDocument ConfigDoc = new XDocument();
            ConfigDoc = ConfigSource.GetConfigFileXmlLinq(); //GetConfigFileXml();
            Nullable<bool> result = Diag.ShowDialog();
            if (result == true)
            {
                string filename = Diag.FileName;
                ConfigDoc.Save(filename);
            }

        }

        private void File_Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConfigSource.SaveConfig(DataSourceType.xml);
        }
        private void File_Import_MenuItem_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void ChangeLanguage(string languagetoset)
        {
            CultureInfo info = new CultureInfo(languagetoset);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = info;
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
            settings.CultureName = info.Name;
            ConfigSource.SaveSettingsToRegistry(settings);
            sconnView view = new sconnView();
            this.Close();
        }


        private void Lang_English_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("en-US");    
        }

        private void Lang_Polish_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("pl-PL");    
        }

        private void Lang_Deutsch_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("de-DE");    
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
        


        }





}//sconn namespace
