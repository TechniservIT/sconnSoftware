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

        public string Dns1 { get; set; }

        public string Dns2 { get; set; }

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
            return _netConfig;
        }

        private byte[] _netConfig;

        public void SetNetworkParams(byte[] netCfg)
        {
            try
            {
                _netConfig = netCfg;

                //load to view
                TbxIpAddrB1.Text = netCfg[0].ToString();
                TbxIpAddrB2.Text = netCfg[1].ToString();
                TbxIpAddrB3.Text = netCfg[2].ToString();
                TbxIpAddrB4.Text = netCfg[3].ToString();

                TbxNetmaskB1.Text = netCfg[4].ToString();
                TbxNetmaskB2.Text = netCfg[5].ToString();
                TbxNetmaskB3.Text = netCfg[6].ToString();
                TbxNetmaskB4.Text = netCfg[7].ToString();

                TbxGatewayB1.Text = netCfg[8].ToString();
                TbxGatewayB2.Text = netCfg[9].ToString();
                TbxGatewayB3.Text = netCfg[10].ToString();
                TbxGatewayB4.Text = netCfg[11].ToString();

                TbxDns1B1.Text = netCfg[12].ToString();
                TbxDns1B2.Text = netCfg[13].ToString();
                TbxDns1B3.Text = netCfg[14].ToString();
                TbxDns1B4.Text = netCfg[15].ToString();


                TbxDns2B1.Text = netCfg[16].ToString();
                TbxDns2B2.Text = netCfg[17].ToString();
                TbxDns2B3.Text = netCfg[18].ToString();
                TbxDns2B4.Text = netCfg[19].ToString();

            }
            catch (Exception e)
            {
                
            }


        }


        private void LoadNetworkParamInput()
        {
            _netConfig[0] = (byte)int.Parse(TbxIpAddrB1.Text);
            _netConfig[1] = (byte)int.Parse(TbxIpAddrB2.Text);
            _netConfig[2] = (byte)int.Parse(TbxIpAddrB3.Text);
            _netConfig[3] = (byte)int.Parse(TbxIpAddrB4.Text);
            _netConfig[4] = (byte)int.Parse(TbxNetmaskB1.Text);
            _netConfig[5] = (byte)int.Parse(TbxNetmaskB2.Text);
            _netConfig[6] = (byte)int.Parse(TbxNetmaskB3.Text);
            _netConfig[7] = (byte)int.Parse(TbxNetmaskB4.Text);
            _netConfig[8] = (byte)int.Parse(TbxGatewayB1.Text);
            _netConfig[9] = (byte)int.Parse(TbxGatewayB2.Text);
            _netConfig[10] = (byte)int.Parse(TbxGatewayB3.Text);
            _netConfig[11] = (byte)int.Parse(TbxGatewayB4.Text);
            _netConfig[12] = (byte)int.Parse(TbxDns1B1.Text);
            _netConfig[13] = (byte)int.Parse(TbxDns1B2.Text);
            _netConfig[14] = (byte)int.Parse(TbxDns1B3.Text);
            _netConfig[15] = (byte)int.Parse(TbxDns1B4.Text);
            _netConfig[16] = (byte)int.Parse(TbxDns2B1.Text);
            _netConfig[17] = (byte)int.Parse(TbxDns2B2.Text);
            _netConfig[18] = (byte)int.Parse(TbxDns2B3.Text);
            _netConfig[19] = (byte)int.Parse(TbxDns2B4.Text);
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
