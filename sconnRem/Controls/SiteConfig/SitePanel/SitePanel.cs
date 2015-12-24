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
        private SitePanelItem[] items;
        private int _itemCount = 0;
        private double _width;
        private double _height;
        private static int selectedItemID = -1;

        public event EventHandler SelectItemChanged;

        public int SelectedItemID { get { return selectedItemID; } }

        public SitePanel()
        {
            //SolidColorBrush myBrush = new SolidColorBrush(Colors.DarkBlue);
            //this.Background = myBrush;
            items = new SitePanelItem[0];
        }

        public SitePanel(double width, double height)
            : this()
        {
            _width = width;
            _height = height;
            this.Width = width;
            this.Height = height;
        }

        public void SelectItem(int itemID)
        {
            if (itemID > -1)
            {
                if (sconnDataShare.GetLastItemID() < itemID) //item to set is higher then last item
                {
                    selectedItemID = sconnDataShare.GetLastItemID();
                }
                selectedItemID = itemID; //getItemIndexByID(siteID);
                SelectItemChanged.Invoke(this, new EventArgs());
            }
        }

        public void ReloadSitesFromShare()
        {
            removeItems();
            sconnSite[] sites = sconnDataShare.getSites();
            foreach (sconnSite site in sites)
            {
                SitePanelItem item = new SitePanelItem(site.siteName, this.Width, this.Height * 0.2, site.siteID);
                this.addStatusItem(item);
            }
        }

        private void setItemSelected(object sender, MouseButtonEventArgs e, int siteID)
        {
            selectedItemID = siteID; //getItemIndexByID(siteID);
            SelectItemChanged.Invoke(this, new EventArgs());
        }

        private int getItemIndexByID(int ID)
        {
            for (int i = 0; i < _itemCount; i++)
            {
                if (items[i].siteID == ID)
                {
                    return i;
                }
            }
            return 0;
        }

        public void addStatusItem(SitePanelItem item)
        {
            try
            {
                _itemCount++;
                SitePanelItem[] ResizedItems = new SitePanelItem[_itemCount];
                for (int i = 0; i < items.GetLength(0); i++)
                {
                    ResizedItems[i] = items[i];
                }
                ResizedItems[_itemCount - 1] = item;
                ResizedItems[_itemCount - 1].MouseDown += new MouseButtonEventHandler((sender, e) => setItemSelected(sender, e, item.siteID));
                items = ResizedItems;
                //check if item is selected and modify 
                if (item.siteID == selectedItemID)
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

        public void removeItems()
        {
            this.Children.Clear();
            _itemCount = 0;
            items = new SitePanelItem[0];
        }

        public void removeStatusItem(int ID)
        {
            if (ID <= 0 && ID > _itemCount - 1)
            {
                return;
            }
            for (int i = ID; i < _itemCount - 2; i++)
            {
                items[i] = items[i + 1];
            }
            _itemCount--;
            SitePanelItem[] ResizedItems = new SitePanelItem[_itemCount];
            items.CopyTo(ResizedItems, 0);
            items = ResizedItems;
        }

        public int itemCount { get { return _itemCount; } }
        public SitePanelItem[] getItems { get { return items; } }
        public SitePanelItem getItem(int i) { return items[i]; }

    }

}
