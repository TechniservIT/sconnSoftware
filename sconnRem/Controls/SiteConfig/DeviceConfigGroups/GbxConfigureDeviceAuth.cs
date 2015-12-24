using sconnConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sconnRem.Controls
{
    public class GbxConfigureAuthDevicesGroup : GroupBox
    {
        public event EventHandler ConfigChanged;

        public int DeviceNo { get; set; }

        private Grid AuthDevCfgGrid;

        private ListView AuthedView;

        private TextBox TbxDeviceIdEntry;

        private Button BtnAddDeviceAuth;

        private Button BtnRmvDeviceAuth;

        private Button BtnEditDeviceAuth;

        public ListViewItem SelectedDevice { get; set; }

        public GbxConfigureAuthDevicesGroup()
        {
            LoadChildControls();
            bool visisble = this.IsVisible;
            this.Header = "Authorized devices";
            this.Content = AuthDevCfgGrid;

        }

        public byte[] Serialize()
        {
            byte[] bytes = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
            for (int i = 0; i < AuthedView.Items.Count; i++)
            {
                ListViewItem item = (ListViewItem)AuthedView.Items[i];
                char[] uuidBytes = item.Content.ToString().ToCharArray();
                for (int j = 0; j < uuidBytes.Length; j++)
                {
                    bytes[i*ipcDefines.SYS_ALRM_UUID_LEN+j]=(byte)uuidBytes[j];
                }

            }
            return bytes;
        }

        public void ReadFromBytes(byte[] bytes)
        {

        }

        private void LoadChildControls()
        {
            try
            {
                AuthDevCfgGrid = new Grid();

      
                AuthDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                AuthDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                AuthDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                AuthDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                AuthDevCfgGrid.RowDefinitions.Add(new RowDefinition());

                ColumnDefinition col1 = new ColumnDefinition();
                ColumnDefinition col2 = new ColumnDefinition();
                AuthDevCfgGrid.ColumnDefinitions.Add(col1);
                AuthDevCfgGrid.ColumnDefinitions.Add(col2);

                this.TbxDeviceIdEntry = new TextBox();
                Grid.SetRow(TbxDeviceIdEntry, 0);
                Grid.SetColumn(TbxDeviceIdEntry, 0);
                AuthDevCfgGrid.Children.Add(TbxDeviceIdEntry);

                this.AuthedView = new ListView();
                Grid.SetRow(AuthedView, 1);
                Grid.SetColumn(AuthedView, 0);
                AuthDevCfgGrid.Children.Add(AuthedView);

                this.BtnAddDeviceAuth = new Button();
                BtnAddDeviceAuth.Content = "Add";
                this.BtnAddDeviceAuth.Click += BtnAddDeviceAuth_Click;
                Grid.SetRow(BtnAddDeviceAuth, 0);
                Grid.SetColumn(BtnAddDeviceAuth, 1);
                AuthDevCfgGrid.Children.Add(BtnAddDeviceAuth);

                this.BtnRmvDeviceAuth = new Button();
                BtnRmvDeviceAuth.Content = "Remove";
                this.BtnRmvDeviceAuth.Click += BtnRmvDeviceAuth_Click;
                Grid.SetRow(BtnRmvDeviceAuth, 1);
                Grid.SetColumn(BtnRmvDeviceAuth, 1);
                AuthDevCfgGrid.Children.Add(BtnRmvDeviceAuth);

                this.BtnEditDeviceAuth = new Button();
                BtnEditDeviceAuth.Content = "Edit";
                this.BtnEditDeviceAuth.Click += BtnEditDeviceAuth_Click;
                Grid.SetRow(BtnEditDeviceAuth, 2);
                Grid.SetColumn(BtnEditDeviceAuth, 1);
                AuthDevCfgGrid.Children.Add(BtnEditDeviceAuth);

            }
            catch (Exception e)
            {
                throw;
            }
            

        }

        void BtnEditDeviceAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = AuthedView.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    TbxDeviceIdEntry.Text = ((ListViewItem)AuthedView.SelectedItem).Content.ToString();
                }
            }
            catch (Exception exc)
            {
                throw;
            }

        }

        void BtnAddDeviceAuth_Click(object sender, RoutedEventArgs e)
        {
            AddRow();
        }

        void BtnRmvDeviceAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = AuthedView.SelectedIndex;

                AuthedView.Items.Remove(AuthedView.SelectedItem);

                // if no rows left, add a blank row
                if (AuthedView.Items.Count == 0)
                {
                    AddRow();
                }
                // otherwise select next row
                else if (selectedIndex <= AuthedView.Items.Count - 1)
                {
                    AuthedView.SelectedIndex = selectedIndex;
                }
                else // not above cases? Select last row
                {
                    AuthedView.SelectedIndex = AuthedView.Items.Count - 1;
                }
            }
            catch (Exception exc)
            {
                throw;
            }

        }

        private void AuthedView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem lvc = (ListViewItem)AuthedView.SelectedItem;
            if (lvc != null)
            {
                TbxDeviceIdEntry.Text = lvc.Content.ToString();
            }
        }



        private void AddRow()
        {
            ListViewItem item = new ListViewItem();
            item.Content = TbxDeviceIdEntry.Text;
            AuthedView.Items.Add(item);
            AuthedView.SelectedIndex = AuthedView.Items.Count - 1;

            TbxDeviceIdEntry.Text = ""; //clear after adding
            TbxDeviceIdEntry.Focus();
        }



    }


}
