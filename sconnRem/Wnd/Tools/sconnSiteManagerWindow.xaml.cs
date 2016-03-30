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
   
    {
        public delegate void AsyncCallback(IAsyncResult ar);
        public delegate void AddTolstDiscoveredDevices(object o);



        struct UdpState
        {
        }

        {
           // site.siteCfg.fillSampleCfg();
            sconnDataShare.addSite(site);
            EditSiteData(site.siteID); //load item for edit
        }

        private void ScanInit()
        {
            try
            {

                // Set the local UDP port to listen on

                // Enable the transmission of broadcast packets without having them be received by ourself

                // Configure ourself to receive discovery responses

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

            // Obtain the UDP message body and convert it to a string, with remote IP address attached as well

            // Configure the UdpClient class to accept more messages, if they arrive

            //Write the received UDP message text to the listbox in a thread-safe manner
            //lstDiscoveredDevices.Invoke(new AddTolstDiscoveredDevices(AddDiscoveryEntry), ReceiveString);
           //listView1.Invoke(new AddTolstDiscoveredDevices(AddDiscoveryEntry), ReceiveString);


            //is not internal address
            foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties prop = netif.GetIPProperties();
                foreach (UnicastIPAddressInformation item in prop.UnicastAddresses)
                {
                    if (!(item.Address.IsIPv6LinkLocal || item.Address.IsIPv6SiteLocal))
                    {

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
                {
                    return;
                }
            }

            //add client , invoke        
        }



        public event EventHandler ConfigChanged;


        {
            InitializeComponent();
            this.Show();
            ScanInit();
        }


        {
            sconnSite[] sites = sconnDataShare.getSites();
            foreach (sconnSite site in sites)
            {
            }
        }

        void sitesPanel_SelectItemChanged(object sender, EventArgs e)
        {
        }


        private void siteEditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigChanged.Invoke(this, new EventArgs());      
            }
            catch (Exception err)
            {
                
              
            }
               
        }

        private void RemoveSiteButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigChanged.Invoke(this, new EventArgs());          
        }

        private void EditSiteButton_Click(object sender, RoutedEventArgs e)
        {
        }

        {
            //load site into gui
        }

        private void AddSiteButton_Click(object sender, RoutedEventArgs e)
        {
            sconnSite site = new sconnSite("DefaultSite", 10000,"192.168.1.100", 37222, "testpass"); //string siteName, int interval, string server, int port, string password
         //   site.siteCfg.fillSampleCfg();  
            sconnDataShare.addSite(site);
            EditSiteData(site.siteID); //load item for edit
        }

        private void SearchSiteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Try to send the discovery request message
            }
            catch
            {
              //  MessageBox.Show("Unable to transmit discovery message.  Check network connectivity and ensure that no other instances of this program are running.", "Error", MessageBoxButtons.OK);
                return;
            }
        }


    }
}
