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
   
    public partial class SconnSiteManagerWindow : Window
    {
        public delegate void AsyncCallback(IAsyncResult ar);
        public delegate void AddTolstDiscoveredDevices(object o);
        public delegate void AddSite(string ip, string hostname);


        private UdpState _globalUdp;

        struct UdpState
        {
            public System.Net.IPEndPoint Ep;
            public System.Net.Sockets.UdpClient UdpClient;
        }

        private void AddSiteToList(string ip, string hostname)
        {
            sconnSite site = new sconnSite(hostname, 10000, ip, 37222, "testpass"); 
           // site.siteCfg.fillSampleCfg();
            sconnDataShare.addSite(site);
            _sitesPanel.ReloadSitesFromShare();
            _sitesPanel.SelectItem(site.siteID); //select current item for edit
            EditSiteData(site.siteID); //load item for edit
            InitSiteList();  //reload gui
        }

        private void ScanInit()
        {
            try
            {
                _globalUdp.UdpClient = new UdpClient();
                _globalUdp.Ep = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 30303);
                System.Net.IPEndPoint bindEp = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 30303);
                byte[] discoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");

                // Set the local UDP port to listen on
                _globalUdp.UdpClient.Client.Bind(bindEp);

                // Enable the transmission of broadcast packets without having them be received by ourself
                _globalUdp.UdpClient.EnableBroadcast = true;
                _globalUdp.UdpClient.MulticastLoopback = false;

                // Configure ourself to receive discovery responses
                _globalUdp.UdpClient.BeginReceive(ReceiveCallback, _globalUdp);

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
                byte[] discoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");
                _globalUdp.UdpClient.Send(discoverMsg, discoverMsg.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("255.255.255.255"), 30303));
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
            UdpState myUdp = (UdpState)ar.AsyncState;

            // Obtain the UDP message body and convert it to a string, with remote IP address attached as well
            string receiveString = Encoding.ASCII.GetString(myUdp.UdpClient.EndReceive(ar, ref myUdp.Ep));
            receiveString = myUdp.Ep.Address.ToString() + "\n" + receiveString.Replace("\r\n", "\n");

            // Configure the UdpClient class to accept more messages, if they arrive
            myUdp.UdpClient.BeginReceive(ReceiveCallback, myUdp);

            //Write the received UDP message text to the listbox in a thread-safe manner
            //lstDiscoveredDevices.Invoke(new AddTolstDiscoveredDevices(AddDiscoveryEntry), ReceiveString);
           //listView1.Invoke(new AddTolstDiscoveredDevices(AddDiscoveryEntry), ReceiveString);
            string[] ePinfo = receiveString.Split('\n');
            string remoteIp = ePinfo[0];
            string remoteHostname = ePinfo[1];


            //is not internal address
            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties prop = netif.GetIPProperties();
                foreach (UnicastIPAddressInformation item in prop.UnicastAddresses)
                {
                    if (!(item.Address.IsIPv6LinkLocal || item.Address.IsIPv6SiteLocal))
                    {

                        if (item.Address.ToString().Equals(remoteIp))
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
                if (remoteIp.Equals(sites[i].serverIP))
                {
                    return;
                }
            }

            //add client , invoke        
            this.Dispatcher.Invoke(new AddSite(AddSiteToList), DispatcherPriority.Normal, new Object[]{remoteIp,remoteHostname});
        }


        private sconnCfgMngr _configManager = new sconnCfgMngr();

        public event EventHandler ConfigChanged;

        SitePanel _sitesPanel;

        public SconnSiteManagerWindow()
        {
            InitializeComponent();
            InitSiteList();
            this.Show();
            ScanInit();
        }


        private void InitSiteList()
        {
            SiteListPanel.Children.Clear();
            sconnSite[] sites = sconnDataShare.getSites();
            _sitesPanel = new SitePanel(SiteListPanel.Width, SiteListPanel.Height);
            foreach (sconnSite site in sites)
            {
                SitePanelItem item = new SitePanelItem(site.siteName, SiteListPanel.Width, SiteListPanel.Height * 0.2,site.siteID);
                _sitesPanel.AddStatusItem(item);
            }
            _sitesPanel.SelectItemChanged += sitesPanel_SelectItemChanged;
            SiteListPanel.Children.Add(_sitesPanel);
        }

        void sitesPanel_SelectItemChanged(object sender, EventArgs e)
        {
            InitSiteList();  
        }


        private void siteEditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int siteId = _sitesPanel.SelectedItemId;
                sconnSite site = sconnDataShare.getSite(siteId);
                site.siteName = SiteNameInput.Text;
                site.serverIP = SiteHostnameInput.Text;
                site.serverPort = int.Parse(SitePortInput.Text);
                site.statusCheckInterval = int.Parse(SiteIntervalInput.Text);
                site.authPasswd = SitePasswordInput.Text;
                _configManager.saveConfig();
                InitSiteList();  //reload gui
                ConfigChanged.Invoke(this, new EventArgs());      
            }
            catch (Exception err)
            {
                
              
            }
               
        }

        private void RemoveSiteButton_Click(object sender, RoutedEventArgs e)
        {
            sconnDataShare.removeSite( _sitesPanel.SelectedItemId);
            _configManager.saveConfig();
            InitSiteList();  //reload gui
            ConfigChanged.Invoke(this, new EventArgs());          
        }

        private void EditSiteButton_Click(object sender, RoutedEventArgs e)
        {
            EditSiteData(_sitesPanel.SelectedItemId);
        }

        private void EditSiteData(int siteId)
        {
            //load site into gui
            sconnSite site = sconnDataShare.getSite(siteId);
            SiteNameInput.Text = site.siteName;
            SiteHostnameInput.Text = site.serverIP;
            SitePortInput.Text = site.serverPort.ToString();
            SiteIntervalInput.Text = site.statusCheckInterval.ToString();
            SitePasswordInput.Text = site.authPasswd;
        }

        private void AddSiteButton_Click(object sender, RoutedEventArgs e)
        {
            sconnSite site = new sconnSite("DefaultSite", 10000,"192.168.1.100", 37222, "testpass"); //string siteName, int interval, string server, int port, string password
         //   site.siteCfg.fillSampleCfg();  
            sconnDataShare.addSite(site);
            _sitesPanel.ReloadSitesFromShare();
            _sitesPanel.SelectItem(site.siteID); //select current item for edit
            EditSiteData(site.siteID); //load item for edit
            InitSiteList();  //reload gui
        }

        private void SearchSiteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Try to send the discovery request message
                byte[] discoverMsg = Encoding.ASCII.GetBytes("Discovery: Who is out there?");
                _globalUdp.UdpClient.Send(discoverMsg, discoverMsg.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.1.255"), 30303));
            }
            catch
            {
              //  MessageBox.Show("Unable to transmit discovery message.  Check network connectivity and ensure that no other instances of this program are running.", "Error", MessageBoxButtons.OK);
                return;
            }
        }


    }
}
