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
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using sconnConnector;
using sconnConnector.POCO.Config;

namespace sconnRem
{
    /// <summary>
    /// Interaction logic for sconnSiteManagerWindow.xaml
    /// </summary>
    /// 
   
    public partial class sconnSiteManagerWindow : Window
    {
        public delegate void AsyncCallback(IAsyncResult ar);
        public delegate void AddTolstDiscoveredDevices(object o);
        public delegate void AddSite(string IP, string Hostname);


        private UdpState GlobalUDP;

        struct UdpState
        {
            public System.Net.IPEndPoint EP;
            public System.Net.Sockets.UdpClient UDPClient;
        }

        private void AddSiteToList(string IP, string Hostname)
        {
            sconnSite site = new sconnSite(Hostname, 10000, IP, 37222, "testpass"); 
           // site.siteCfg.fillSampleCfg();
            sconnDataShare.addSite(site);
            sitesPanel.ReloadSitesFromShare();
            sitesPanel.SelectItem(site.siteID); //select current item for edit
            EditSiteData(site.siteID); //load item for edit
            initSiteList();  //reload gui
        }

        private void ScanInit()
        {
            try
            {
                GlobalUDP.UDPClient = new UdpClient();
                GlobalUDP.EP = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 30303);
                System.Net.IPEndPoint BindEP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 30303);
                byte[] DiscoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");

                // Set the local UDP port to listen on
                GlobalUDP.UDPClient.Client.Bind(BindEP);

                // Enable the transmission of broadcast packets without having them be received by ourself
                GlobalUDP.UDPClient.EnableBroadcast = true;
                GlobalUDP.UDPClient.MulticastLoopback = false;

                // Configure ourself to receive discovery responses
                GlobalUDP.UDPClient.BeginReceive(ReceiveCallback, GlobalUDP);

                // Transmit the discovery request message
               // GlobalUDP.UDPClient.Send(DiscoverMsg, DiscoverMsg.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 30303));
            }
            catch
            {
                //MessageBox.Show("Unable to transmit discovery message.  Check network connectivity and ensure that no other instances of this program are running.", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void cmdDiscoverDevices_Click(object sender, EventArgs e)
        {
            try
            {

                // Try to send the discovery request message
                byte[] DiscoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");
                GlobalUDP.UDPClient.Send(DiscoverMsg, DiscoverMsg.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 30303));
            }
            catch
            {
            //    MessageBox.Show("Unable to transmit discovery message.  Check network connectivity and ensure that no other instances of this program are running.", "Error", MessageBoxButtons.OK);
                return;
            }
        }

        public void AddDiscoveryEntry(object o)
        {
            //lstDiscoveredDevices.Items.Add(o);
          //  listView1.Items.Add(new ListViewItem(((string)(o)).Split('\n')));
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            UdpState MyUDP = (UdpState)ar.AsyncState;

            // Obtain the UDP message body and convert it to a string, with remote IP address attached as well
            string ReceiveString = Encoding.ASCII.GetString(MyUDP.UDPClient.EndReceive(ar, ref MyUDP.EP));
            ReceiveString = MyUDP.EP.Address.ToString() + "\n" + ReceiveString.Replace("\r\n", "\n");

            // Configure the UdpClient class to accept more messages, if they arrive
            MyUDP.UDPClient.BeginReceive(ReceiveCallback, MyUDP);

            //Write the received UDP message text to the listbox in a thread-safe manner
            //lstDiscoveredDevices.Invoke(new AddTolstDiscoveredDevices(AddDiscoveryEntry), ReceiveString);
           //listView1.Invoke(new AddTolstDiscoveredDevices(AddDiscoveryEntry), ReceiveString);
            string[] EPinfo = ReceiveString.Split('\n');
            string RemoteIp = EPinfo[0];
            string RemoteHostname = EPinfo[1];


            //is not internal address
            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties prop = netif.GetIPProperties();
                foreach (UnicastIPAddressInformation item in prop.UnicastAddresses)
                {
                    if (!(item.Address.IsIPv6LinkLocal || item.Address.IsIPv6SiteLocal))
                    {

                        if (item.Address.ToString().Equals(RemoteIp))
                        {
                            return;
                        }
                    }
                }
            }

            //verify client is not already known
            sconnSite[] sites = sconnDataShare.getSites();
            for (int i = 0; i < sites.Length; i++)
            {
                if (RemoteIp.Equals(sites[i].serverIP))
                {
                    return;
                }
            }

            //add client , invoke        
            this.Dispatcher.Invoke(new AddSite(AddSiteToList), DispatcherPriority.Normal, new Object[]{RemoteIp,RemoteHostname});
        }


        private sconnCfgMngr ConfigManager = new sconnCfgMngr();

        public event EventHandler ConfigChanged;

        SitePanel sitesPanel;

        public sconnSiteManagerWindow()
        {
            InitializeComponent();
            initSiteList();
            this.Show();
            ScanInit();
        }


        private void initSiteList()
        {
            siteListPanel.Children.Clear();
            sconnSite[] sites = sconnDataShare.getSites();
            sitesPanel = new SitePanel(siteListPanel.Width, siteListPanel.Height);
            foreach (sconnSite site in sites)
            {
                SitePanelItem item = new SitePanelItem(site.siteName, siteListPanel.Width, siteListPanel.Height * 0.2,site.siteID);
                sitesPanel.addStatusItem(item);
            }
            sitesPanel.SelectItemChanged += sitesPanel_SelectItemChanged;
            siteListPanel.Children.Add(sitesPanel);
        }

        void sitesPanel_SelectItemChanged(object sender, EventArgs e)
        {
            initSiteList();  
        }


        private void siteEditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int siteID = sitesPanel.SelectedItemID;
                sconnSite site = sconnDataShare.getSite(siteID);
                site.siteName = siteNameInput.Text;
                site.serverIP = siteHostnameInput.Text;
                site.serverPort = int.Parse(sitePortInput.Text);
                site.statusCheckInterval = int.Parse(siteIntervalInput.Text);
                site.authPasswd = sitePasswordInput.Text;
                ConfigManager.saveConfig();
                initSiteList();  //reload gui
                ConfigChanged.Invoke(this, new EventArgs());      
            }
            catch (Exception err)
            {
                
              
            }
               
        }

        private void RemoveSiteButton_Click(object sender, RoutedEventArgs e)
        {
            sconnDataShare.removeSite( sitesPanel.SelectedItemID);
            ConfigManager.saveConfig();
            initSiteList();  //reload gui
            ConfigChanged.Invoke(this, new EventArgs());          
        }

        private void EditSiteButton_Click(object sender, RoutedEventArgs e)
        {
            EditSiteData(sitesPanel.SelectedItemID);
        }

        private void EditSiteData(int siteID)
        {
            //load site into gui
            sconnSite site = sconnDataShare.getSite(siteID);
            siteNameInput.Text = site.siteName;
            siteHostnameInput.Text = site.serverIP;
            sitePortInput.Text = site.serverPort.ToString();
            siteIntervalInput.Text = site.statusCheckInterval.ToString();
            sitePasswordInput.Text = site.authPasswd;
        }

        private void AddSiteButton_Click(object sender, RoutedEventArgs e)
        {
            sconnSite site = new sconnSite("DefaultSite", 10000,"192.168.1.100", 37222, "testpass"); //string siteName, int interval, string server, int port, string password
         //   site.siteCfg.fillSampleCfg();  
            sconnDataShare.addSite(site);
            sitesPanel.ReloadSitesFromShare();
            sitesPanel.SelectItem(site.siteID); //select current item for edit
            EditSiteData(site.siteID); //load item for edit
            initSiteList();  //reload gui
        }

        private void SearchSiteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Try to send the discovery request message
                byte[] DiscoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");
                GlobalUDP.UDPClient.Send(DiscoverMsg, DiscoverMsg.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.1.255"), 30303));
            }
            catch
            {
              //  MessageBox.Show("Unable to transmit discovery message.  Check network connectivity and ensure that no other instances of this program are running.", "Error", MessageBoxButtons.OK);
                return;
            }
        }


    }
}
