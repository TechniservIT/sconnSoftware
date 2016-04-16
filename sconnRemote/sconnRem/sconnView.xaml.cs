﻿using System;
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
using sconnConnector.Config;
using iotDbConnector.DAL;
using Microsoft.Practices.Unity;

namespace sconnRem
{

    public  partial class SconnView : Window
    {
        
        private static System.Timers.Timer _sconnTask;
        //private static StackPanel dataViewPanel;
        private static double _viewHeight;
        private static double _viewWidth;
        private int _viewedSiteId = 0;
        private sconnDataSrc _configSource  = new sconnDataSrc();
        private SconnSecurityDialog _seDiag;
        private SconnSiteManagerWindow _siteManagerWnd;
        private StatusViewPanel _statusView;
        private SitePanel _sitesPanel;
        private SiteView _siteView;
        private SiteEditView _siteEditView;
        private MapWindow _mapView;
        private sconnCfgMngr _configManager = new sconnCfgMngr();
        private GlobalSettings _settings = new GlobalSettings();

        public static double ViewHeight { get { return _viewHeight; } set { _viewHeight = value; } }
        public static double ViewWidth { get { return _viewWidth; } set { _viewWidth = value; } }

        private delegate void UpdateSiteViewDelegate(); //site status update del function

        public void PopulateDataView(StackPanel panel)
        {
            DataView.Children.Clear();
            DataView.Children.Add(panel);
        }

        private void Bootstrap_FindAndLoad_UsbDevices()
        {
            USB usbcomm = new USB();
             //   usbcomm.UsbComm_Test_Trx();
            //  usbcomm.UsbComm_Sample_Trx();
            bool usbConn = usbcomm.TestConnection();
            if (usbConn)
            {
                sconnSite site = new sconnSite("USB_PROG",400,"",0,"");   //string siteName, int intervalMs, string server, int port, string password)
                site.UsbCom = true;
                sconnDataShare.addSite(site);
                _configManager.saveConfig();
            }


            // string resp = usbcomm.ReadUsbBlocking();
            // usbcomm.TransmitLoop();
        }

        public SconnView()
        {
          
            _configSource.SetXmlPath(Directory.GetCurrentDirectory().ToString());
            _configSource.SetXmlFileName("sconnRem.xml");
            _settings = _configSource.LoadRegistryData();

            CultureInfo info = new CultureInfo(_settings.CultureName);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = info; //CultureInfo.InstalledUICulture;
            Thread.CurrentThread.CurrentUICulture = info;
            Thread.CurrentThread.CurrentCulture = info;

            //prompt for access before init
            _seDiag = new SconnSecurityDialog();
            _seDiag.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
           // seDiag.Topmost = true;
            _seDiag.Closed += seDiag_Closed;
            _seDiag.Closing += seDiag_Closing;
            _seDiag.KeyDown += seDiag_KeyDown;
            _seDiag.LoginButton.Click += loginButton_Click;


            Bootstrap_FindAndLoad_UsbDevices();


            //mapView = new MapWindow();
            //mapView.Show();

            _statusView = new StatusViewPanel();

            InitMainView();
            this.Hide(); //hide gui until login 


        }


        private void InitMainView()
        {
            InitSiteConfig();
            InitializeComponent();
            InitSiteList();
           
            ViewHeight = DataView.Height;
            ViewWidth = DataView.Width;
        }
      

        private void InitSiteConfig()
        {

            /**** site config is read from file no need to add */
            _configSource.LoadConfig(DataSourceType.xml);
            //sconnDataShare.removeSites();

            _sconnTask = new System.Timers.Timer(1000);
            _sconnTask.Elapsed += new ElapsedEventHandler(ProcessSiteUpdates);
            _sconnTask.Interval = 1000;
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
                int currentTabId = 0;//default is first tab;
                if (_siteView != null)
                {
                    currentTabId =  _siteView.SiteTabView.SelectedIndex;
                }
                _siteView = new SiteView(_viewedSiteId);
                _siteView.SiteTabView.SelectedIndex = currentTabId;
                DataView.Children.Clear();
                DataView.Children.Add(_siteView);
            }
        }



        private void LoadSiteEdit()
        {
            _siteEditView = new SiteEditView(_viewedSiteId);
            DataView.Children.Clear();
            DataView.Children.Add(_siteEditView);
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
                            Thread updateThread = new Thread(() => UpdateSite( sites[currentSite]));         
                            updateThread.Start();
                        }

                    }
                } 
                //run gui update delegate after sites are refereshed
                DataView.Dispatcher.Invoke(new UpdateSiteViewDelegate(LoadSiteView));

            }
            catch (Exception)
            {        
                throw;
            }
        }


        private void UpdateSite( sconnSite site)
        {
            _configManager.updateSiteConfig( site);  //update status for each device ( read IO registers )
        }

        private void UpdateSites()
        {
            sconnSite[] sites = sconnDataShare.getSites();
            for (int i = 0; i < sites.GetLength(0); i++)
			    {
                    _configManager.updateSiteConfig( sites[i]); //update status for each device ( read IO registers )
			    }
        }

        private bool IsUserValid(string username, string password)
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
            string login = _seDiag.UsernameInput.Text;
            string password = _seDiag.PasswordInput.Text;
            if (login != null && password != null)
            {
                if (IsUserValid(login, password))
                {
                    //TODO : check user permissions
                    _seDiag.UserValidated = true;
                    this.Show();
                    _seDiag.Close();
                }
            }
        }


        void seDiag_Closing(object sender, EventArgs e)
        {
            if (!_seDiag.UserValidated)
            {
                this.Close(); //close parent only if user did not auth
            }
        }

        void seDiag_Closed(object sender, EventArgs e)
        {
    
        }

        public  void SetMainView(StackPanel viewContent)
        {
            DataView.Children.Clear();
            DataView.Children.Add(viewContent);
        }

        private void InitSiteList()
        {
            SiteList.Children.Clear();
            sconnSite[] sites = sconnDataShare.getSites();
            _sitesPanel = new SitePanel(SiteList.Width, SiteList.Height); 
            foreach (sconnSite site in sites)
            {
                SitePanelItem item = new SitePanelItem(site.siteName, SiteList.Width, SiteList.Height *0.1, site.siteID);
                item.SiteBtn1.Click += new RoutedEventHandler((sender, e) => ViewSiteClick(sender, e, item.SiteName));
                item.SiteBtn2.Click += new RoutedEventHandler((sender, e) => ConfigSiteClick(sender, e, item.SiteName));
                item.SiteBtn3.Click += new RoutedEventHandler((sender, e) => EditSiteClick(sender, e, item.SiteName));
                //siteList.Children.Add(item);
                _sitesPanel.AddStatusItem(item);
            }
            _sitesPanel.SelectItemChanged += sitesPanel_SelectItemChanged;        
            SiteList.Children.Add(_sitesPanel);
            //ImageBrush img1 = new ImageBrush();
            //ImageBrush img2 = new ImageBrush();
            //ImageBrush img3 = new ImageBrush();
            //img1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/cog3.jpg", UriKind.Absolute));
            //btnSites.Background = img1;
            //img2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/key1.jpg", UriKind.Absolute));
            //btnConfig.Background = img2;
            //img3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/stat1.jpg", UriKind.Absolute));
            //btnMap.Background = img3;

        }

        void sitesPanel_SelectItemChanged(object sender, EventArgs e)
        {
            InitSiteList();  
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
        private void ConfigureSiteThread(int siteId)
        {
            Thread updateThread = new Thread(() => ConfigureSite(siteId));
            updateThread.Start();
        } 
    
        private void ConfigureSite(int siteId)
        {
            //write config to remote device
            sconnSite site = sconnDataShare.getSite(siteId);

            _configManager.WriteGlobalCfg( site);
            _configManager.WriteDeviceCfg( site);
        }




        void SiteLoadingFinished(object sender, EventArgs e, bool success, int siteId)
        {
            if ( success == true)
            {
                sconnDataShare.SiteLiveViewEnabled = true;
                _viewedSiteId = siteId;
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
                        DataView.Children.Clear();
                        DataView.Children.Add(status);
                        status.TestConnection();
                      
                    }
                }
                index++;
            }

        }

        private void EditSiteClick(object sender, RoutedEventArgs e, string name)
        {


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
                            _viewedSiteId = sconnSite.siteID;

                            //LoadSiteEdit();


                            //wndConfigureSiteShell wnd = new wndConfigureSiteShell();

                            //EndpointInfo info = new EndpointInfo();
                            //info.Hostname = sconnSite.serverIP;
                            //info.Port = sconnSite.serverPort;
                            //DeviceCredentials cred = new DeviceCredentials();
                            //cred.Password = sconnSite.authPasswd;
                            //cred.Username = "";
                            //AlarmSystemConfigManager manager = new AlarmSystemConfigManager(info, cred);
                            //Device alrmSysDev = new Device();
                            //alrmSysDev.Credentials = cred;
                            //alrmSysDev.EndpInfo = info;
                            //manager.RemoteDevice = alrmSysDev;


                            //ConfigNavBootstrapper bootstrapper = new ConfigNavBootstrapper(manager);
                            //bootstrapper.Run();

                            //ConfigureSiteViewModel context = new ConfigureSiteViewModel(_Manager);
                            //wnd.DataContext = context;
                            //wnd.Show();


                        }
                        else
                        {
                            _statusView.SetStatusText("Błąd połączenia");
                            DataView.Children.Clear();
                            DataView.Children.Add(_statusView);
                        }
                    }
                }
            }

        }

        private void setupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _siteManagerWnd = new SconnSiteManagerWindow();
            _siteManagerWnd.ConfigChanged += siteManagerWnd_ConfigChanged;
        }

        void siteManagerWnd_ConfigChanged(object sender, EventArgs e)
        {
            //reload sitelist when config was changed by config manager
            InitSiteList();
        }

        private void saveConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _configSource.SaveConfig(DataSourceType.xml);
        }

        private void LangSelectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
        

        private void fileMenuItem_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void File_SaveAs_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog diag = new Microsoft.Win32.SaveFileDialog();
            diag.FileName = "sconnRem_" + DateTime.Now.ToShortDateString();
            diag.DefaultExt = ".xml";
            diag.Filter = "Xml Document (*.xml)|*.xml|All files (*.*)|*.*";

            XDocument configDoc = new XDocument();
            configDoc = _configSource.GetConfigFileXmlLinq(); //GetConfigFileXml();
            Nullable<bool> result = diag.ShowDialog();
            if (result == true)
            {
                string filename = diag.FileName;
                configDoc.Save(filename);
            }

        }

        private void File_Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _configSource.SaveConfig(DataSourceType.xml);
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
            _settings.CultureName = info.Name;
            _configSource.SaveSettingsToRegistry(_settings);
            SconnView view = new SconnView();
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

        


        }





}//sconn namespace
