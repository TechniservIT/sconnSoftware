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
        public Button SiteBtn1;
        public Button SiteBtn2;
        public Button SiteBtn3;

        private string _siteName;
        public string SiteName { get { return _siteName; } }

        private int _siteId = 0;
        public int SiteId { get { return _siteId; } }

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

            SiteBtn1 = new Button();
            SiteBtn1.Content = Properties.Resources.btnViewSiteDesc;
            SiteBtn1.Width = this.Width * 0.33;
            SiteBtn1.Height = this.Height * 0.5;
       //     siteBtn1.Click += new RoutedEventHandler((sender, e) => ViewSiteClick(sender, e, name));
            Grid.SetRow(SiteBtn1, 1);
            Grid.SetColumn(SiteBtn1, 0);
            siteGrid.Children.Add(SiteBtn1);

            SiteBtn2 = new Button();
            SiteBtn2.Content = Properties.Resources.btnConfSiteDesc;
            SiteBtn2.Width = this.Width * 0.33;
            SiteBtn2.Height = this.Height * 0.5;
      //      siteBtn2.Click += new RoutedEventHandler((sender, e) => ConfigSiteClick(sender, e, name));
            Grid.SetRow(SiteBtn2, 1);
            Grid.SetColumn(SiteBtn2, 1);
            siteGrid.Children.Add(SiteBtn2);

            SiteBtn3 = new Button();
    //            <StackPanel>
    //    <Image Source="Pictures/apple.jpg" />
    //    <TextBlock>Disconnect from Server</TextBlock>
    //</StackPanel>

            SiteBtn3.Content  = Properties.Resources.btnConfSiteEdit;
            SiteBtn3.Width = this.Width * 0.33;
            SiteBtn3.Height = this.Height * 0.5;
            Grid.SetRow(SiteBtn3, 1);
            Grid.SetColumn(SiteBtn3, 2);
            siteGrid.Children.Add(SiteBtn3);

            Label siteDesc = new Label();
            siteDesc.Content = _siteName;
            siteDesc.Height = this.Height * 0.5;
            siteDesc.Width = this.Width;
            Grid.SetRow(siteDesc, 0);
            Grid.SetColumn(siteDesc, 0);
            siteGrid.Children.Add(siteDesc);
            
            SolidColorBrush myBrushBtnBack = new SolidColorBrush(Colors.SkyBlue);
            SiteBtn1.Background = myBrushBtnBack;
            SiteBtn2.Background = myBrushBtnBack;
            SiteBtn3.Background = myBrushBtnBack;

            SolidColorBrush myBrush = new SolidColorBrush(Colors.LightBlue);
            this.Background = myBrush;
            this.Children.Add(siteGrid);

        }

        public SitePanelItem(string name, double width, double height, int siteId)
            : this(name, width, height)
        {
            _siteId = siteId;
        }

    }

}
