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

        private Grid _authDevCfgGrid;

        private ListView _authedView;

        private TextBox _tbxDeviceIdEntry;

        private Button _btnAddDeviceAuth;

        private Button _btnRmvDeviceAuth;

        private Button _btnEditDeviceAuth;

        public ListViewItem SelectedDevice { get; set; }

        public GbxConfigureAuthDevicesGroup()
        {
            LoadChildControls();
            bool visisble = this.IsVisible;
            this.Header = "Authorized devices";
            this.Content = _authDevCfgGrid;

        }

        public byte[] Serialize()
        {
            byte[] bytes = new byte[ipcDefines.SYS_ALARM_DEV_AUTH_MEM_SIZE];
            for (int i = 0; i < _authedView.Items.Count; i++)
            {
                ListViewItem item = (ListViewItem)_authedView.Items[i];
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
                _authDevCfgGrid = new Grid();

      
                _authDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                _authDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                _authDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                _authDevCfgGrid.RowDefinitions.Add(new RowDefinition());
                _authDevCfgGrid.RowDefinitions.Add(new RowDefinition());

                ColumnDefinition col1 = new ColumnDefinition();
                ColumnDefinition col2 = new ColumnDefinition();
                _authDevCfgGrid.ColumnDefinitions.Add(col1);
                _authDevCfgGrid.ColumnDefinitions.Add(col2);

                this._tbxDeviceIdEntry = new TextBox();
                Grid.SetRow(_tbxDeviceIdEntry, 0);
                Grid.SetColumn(_tbxDeviceIdEntry, 0);
                _authDevCfgGrid.Children.Add(_tbxDeviceIdEntry);

                this._authedView = new ListView();
                Grid.SetRow(_authedView, 1);
                Grid.SetColumn(_authedView, 0);
                _authDevCfgGrid.Children.Add(_authedView);

                this._btnAddDeviceAuth = new Button();
                _btnAddDeviceAuth.Content = "Add";
                this._btnAddDeviceAuth.Click += BtnAddDeviceAuth_Click;
                Grid.SetRow(_btnAddDeviceAuth, 0);
                Grid.SetColumn(_btnAddDeviceAuth, 1);
                _authDevCfgGrid.Children.Add(_btnAddDeviceAuth);

                this._btnRmvDeviceAuth = new Button();
                _btnRmvDeviceAuth.Content = "Remove";
                this._btnRmvDeviceAuth.Click += BtnRmvDeviceAuth_Click;
                Grid.SetRow(_btnRmvDeviceAuth, 1);
                Grid.SetColumn(_btnRmvDeviceAuth, 1);
                _authDevCfgGrid.Children.Add(_btnRmvDeviceAuth);

                this._btnEditDeviceAuth = new Button();
                _btnEditDeviceAuth.Content = "Edit";
                this._btnEditDeviceAuth.Click += BtnEditDeviceAuth_Click;
                Grid.SetRow(_btnEditDeviceAuth, 2);
                Grid.SetColumn(_btnEditDeviceAuth, 1);
                _authDevCfgGrid.Children.Add(_btnEditDeviceAuth);

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
                int selectedIndex = _authedView.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    _tbxDeviceIdEntry.Text = ((ListViewItem)_authedView.SelectedItem).Content.ToString();
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
                int selectedIndex = _authedView.SelectedIndex;

                _authedView.Items.Remove(_authedView.SelectedItem);

                // if no rows left, add a blank row
                if (_authedView.Items.Count == 0)
                {
                    AddRow();
                }
                // otherwise select next row
                else if (selectedIndex <= _authedView.Items.Count - 1)
                {
                    _authedView.SelectedIndex = selectedIndex;
                }
                else // not above cases? Select last row
                {
                    _authedView.SelectedIndex = _authedView.Items.Count - 1;
                }
            }
            catch (Exception exc)
            {
                throw;
            }

        }

        private void AuthedView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem lvc = (ListViewItem)_authedView.SelectedItem;
            if (lvc != null)
            {
                _tbxDeviceIdEntry.Text = lvc.Content.ToString();
            }
        }



        private void AddRow()
        {
            ListViewItem item = new ListViewItem();
            item.Content = _tbxDeviceIdEntry.Text;
            _authedView.Items.Add(item);
            _authedView.SelectedIndex = _authedView.Items.Count - 1;

            _tbxDeviceIdEntry.Text = ""; //clear after adding
            _tbxDeviceIdEntry.Focus();
        }



    }


}
