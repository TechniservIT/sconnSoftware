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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sconnRem.Controls.SiteConfig.NetworkSetup
{
    /// <summary>
    /// Interaction logic for DeviceNetworkConfig.xaml
    /// </summary>
    public partial class DeviceNetworkConfig : UserControl
    {
        public string Ip { get; set; }
        public string NetMask { get; set; }

        public string Gateway { get; set; }

        public string DNS1 { get; set; }

        public string DNS2 { get; set; }

        public DeviceNetworkConfig()
        {
            InitializeComponent();
        }

        public bool IsNetworkConfigCorrect()
        {
            return true;
        }

        public DeviceNetworkConfig(byte[] cfg) :this()
        {
            SetNetworkParams(cfg);
        }

        public byte[] GetNetworkConfig()
        {
            LoadNetworkParamInput();
            return NetConfig;
        }

        private byte[] NetConfig;

        public void SetNetworkParams(byte[] NetCfg)
        {
            try
            {
                NetConfig = NetCfg;

                //load to view
                TbxIpAddrB1.Text = NetCfg[0].ToString();
                TbxIpAddrB2.Text = NetCfg[1].ToString();
                TbxIpAddrB3.Text = NetCfg[2].ToString();
                TbxIpAddrB4.Text = NetCfg[3].ToString();

                TbxNetmaskB1.Text = NetCfg[4].ToString();
                TbxNetmaskB2.Text = NetCfg[5].ToString();
                TbxNetmaskB3.Text = NetCfg[6].ToString();
                TbxNetmaskB4.Text = NetCfg[7].ToString();

                TbxGatewayB1.Text = NetCfg[8].ToString();
                TbxGatewayB2.Text = NetCfg[9].ToString();
                TbxGatewayB3.Text = NetCfg[10].ToString();
                TbxGatewayB4.Text = NetCfg[11].ToString();

                TbxDNS1B1.Text = NetCfg[12].ToString();
                TbxDNS1B2.Text = NetCfg[13].ToString();
                TbxDNS1B3.Text = NetCfg[14].ToString();
                TbxDNS1B4.Text = NetCfg[15].ToString();


                TbxDNS2B1.Text = NetCfg[16].ToString();
                TbxDNS2B2.Text = NetCfg[17].ToString();
                TbxDNS2B3.Text = NetCfg[18].ToString();
                TbxDNS2B4.Text = NetCfg[19].ToString();

            }
            catch (Exception e)
            {
                
            }


        }


        private void LoadNetworkParamInput()
        {
            NetConfig[0] = (byte)int.Parse(TbxIpAddrB1.Text);
            NetConfig[1] = (byte)int.Parse(TbxIpAddrB2.Text);
            NetConfig[2] = (byte)int.Parse(TbxIpAddrB3.Text);
            NetConfig[3] = (byte)int.Parse(TbxIpAddrB4.Text);
            NetConfig[4] = (byte)int.Parse(TbxNetmaskB1.Text);
            NetConfig[5] = (byte)int.Parse(TbxNetmaskB2.Text);
            NetConfig[6] = (byte)int.Parse(TbxNetmaskB3.Text);
            NetConfig[7] = (byte)int.Parse(TbxNetmaskB4.Text);
            NetConfig[8] = (byte)int.Parse(TbxGatewayB1.Text);
            NetConfig[9] = (byte)int.Parse(TbxGatewayB2.Text);
            NetConfig[10] = (byte)int.Parse(TbxGatewayB3.Text);
            NetConfig[11] = (byte)int.Parse(TbxGatewayB4.Text);
            NetConfig[12] = (byte)int.Parse(TbxDNS1B1.Text);
            NetConfig[13] = (byte)int.Parse(TbxDNS1B2.Text);
            NetConfig[14] = (byte)int.Parse(TbxDNS1B3.Text);
            NetConfig[15] = (byte)int.Parse(TbxDNS1B4.Text);
            NetConfig[16] = (byte)int.Parse(TbxDNS2B1.Text);
            NetConfig[17] = (byte)int.Parse(TbxDNS2B2.Text);
            NetConfig[18] = (byte)int.Parse(TbxDNS2B3.Text);
            NetConfig[19] = (byte)int.Parse(TbxDNS2B4.Text);
        }

        private void BtnSaveNetCfg_Click(object sender, RoutedEventArgs e)
        {
            LoadNetworkParamInput();

            //TODO validate input

            //upload config


            //reload 
        }

    }
}
