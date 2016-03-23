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
namespace sconnRem
{

    public class SitePanelItem : StackPanel
    {
        public Button siteBtn1;
        public Button siteBtn2;
        public Button siteBtn3;

        private string _siteName;
        public string siteName { get { return _siteName; } }

        private int _siteID = 0;
        public int siteID { get { return _siteID; } }

        public SitePanelItem(string name, double width, double height)
        {
            _siteName = name;

            Grid siteGrid = new Grid();
            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();

            siteGrid.ColumnDefinitions.Add(colDef1);
            siteGrid.ColumnDefinitions.Add(colDef2);
            siteGrid.ColumnDefinitions.Add(colDef3);


            siteGrid.RowDefinitions.Add(rowDef1);
            siteGrid.RowDefinitions.Add(rowDef2);

            this.Height = height;
            this.Width = width;

            siteBtn1 = new Button();
            siteBtn1.Content = Properties.Resources.btnViewSiteDesc;
            siteBtn1.Width = this.Width * 0.33;
            siteBtn1.Height = this.Height * 0.5;
       //     siteBtn1.Click += new RoutedEventHandler((sender, e) => ViewSiteClick(sender, e, name));
            Grid.SetRow(siteBtn1, 1);
            Grid.SetColumn(siteBtn1, 0);
            siteGrid.Children.Add(siteBtn1);

            siteBtn2 = new Button();
            siteBtn2.Content = Properties.Resources.btnConfSiteDesc;
            siteBtn2.Width = this.Width * 0.33;
            siteBtn2.Height = this.Height * 0.5;
      //      siteBtn2.Click += new RoutedEventHandler((sender, e) => ConfigSiteClick(sender, e, name));
            Grid.SetRow(siteBtn2, 1);
            Grid.SetColumn(siteBtn2, 1);
            siteGrid.Children.Add(siteBtn2);

            siteBtn3 = new Button();
            siteBtn3.Content  = Properties.Resources.btnConfSiteDesc;
            siteBtn3.Width = this.Width * 0.33;
            siteBtn3.Height = this.Height * 0.5;
            Grid.SetRow(siteBtn3, 1);
            Grid.SetColumn(siteBtn3, 2);
            siteGrid.Children.Add(siteBtn3);

            Label siteDesc = new Label();
            siteDesc.Content = _siteName;
            siteDesc.Height = this.Height * 0.5;
            siteDesc.Width = this.Width;
            Grid.SetRow(siteDesc, 0);
            Grid.SetColumn(siteDesc, 0);
            siteGrid.Children.Add(siteDesc);
            
            SolidColorBrush myBrushBtnBack = new SolidColorBrush(Colors.SkyBlue);
            siteBtn1.Background = myBrushBtnBack;
            siteBtn2.Background = myBrushBtnBack;
            siteBtn3.Background = myBrushBtnBack;

            SolidColorBrush myBrush = new SolidColorBrush(Colors.LightBlue);
            this.Background = myBrush;
            this.Children.Add(siteGrid);

        }

        public SitePanelItem(string name, double width, double height, int siteID)
            : this(name, width, height)
        {
            _siteID = siteID;
        }

    }

}
