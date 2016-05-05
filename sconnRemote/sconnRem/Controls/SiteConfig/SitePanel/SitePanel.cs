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
using sconnConnector;
using sconnConnector.POCO.Config;


namespace sconnRem
{



    public class SitePanel : StackPanel
    {
        private SitePanelItem[] _items;
        private int _itemCount = 0;
        private double _width;
        private double _height;
        private static int _selectedItemId = -1;

        public event EventHandler SelectItemChanged;

        public int SelectedItemId { get { return _selectedItemId; } }

        public SitePanel()
        {
            //SolidColorBrush myBrush = new SolidColorBrush(Colors.DarkBlue);
            //this.Background = myBrush;
            _items = new SitePanelItem[0];
        }

        public SitePanel(double width, double height)
            : this()
        {
            _width = width;
            _height = height;
            this.Width = width;
            this.Height = height;
        }

        public void SelectItem(int itemId)
        {
            if (itemId > -1)
            {
                //if (sconnDataShare.GetLastItemID() < itemId) //item to set is higher then last item
                //{
                //    _selectedItemId = sconnDataShare.GetLastItemID();
                //}
                _selectedItemId = itemId; //getItemIndexByID(siteID);
                SelectItemChanged.Invoke(this, new EventArgs());
            }
        }

        public void ReloadSitesFromShare()
        {
            RemoveItems();
            sconnSite[] sites = sconnDataShare.sconnSites.ToArray();
            foreach (sconnSite site in sites)
            {
                SitePanelItem item = new SitePanelItem(site.siteName, this.Width, this.Height * 0.2, site.siteID);
                this.AddStatusItem(item);
            }
        }

        private void SetItemSelected(object sender, MouseButtonEventArgs e, int siteId)
        {
            _selectedItemId = siteId; //getItemIndexByID(siteID);
            SelectItemChanged.Invoke(this, new EventArgs());
        }

        private int GetItemIndexById(int id)
        {
            for (int i = 0; i < _itemCount; i++)
            {
                if (_items[i].SiteId == id)
                {
                    return i;
                }
            }
            return 0;
        }

        public void AddStatusItem(SitePanelItem item)
        {
            try
            {
                _itemCount++;
                SitePanelItem[] resizedItems = new SitePanelItem[_itemCount];
                for (int i = 0; i < _items.GetLength(0); i++)
                {
                    resizedItems[i] = _items[i];
                }
                resizedItems[_itemCount - 1] = item;
                resizedItems[_itemCount - 1].MouseDown += new MouseButtonEventHandler((sender, e) => SetItemSelected(sender, e, item.SiteId));
                _items = resizedItems;
                //check if item is selected and modify 
                if (item.SiteId == _selectedItemId)
                {
                    item.Background = new SolidColorBrush(Colors.Aqua);
                }
                this.Children.Add(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveItems()
        {
            this.Children.Clear();
            _itemCount = 0;
            _items = new SitePanelItem[0];
        }

        public void RemoveStatusItem(int id)
        {
            if (id <= 0 && id > _itemCount - 1)
            {
                return;
            }
            for (int i = id; i < _itemCount - 2; i++)
            {
                _items[i] = _items[i + 1];
            }
            _itemCount--;
            SitePanelItem[] resizedItems = new SitePanelItem[_itemCount];
            _items.CopyTo(resizedItems, 0);
            _items = resizedItems;
        }

        public int ItemCount { get { return _itemCount; } }
        public SitePanelItem[] GetItems { get { return _items; } }
        public SitePanelItem GetItem(int i) { return _items[i]; }

    }

}
