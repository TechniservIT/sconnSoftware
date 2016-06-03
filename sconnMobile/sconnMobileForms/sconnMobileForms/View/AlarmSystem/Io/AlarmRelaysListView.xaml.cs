using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem.Controls;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{


    public partial class AlarmRelaysListView : ContentPage
    {
        public sconnSite Site { get; set; }
        public List<sconnDevice> Configs { get; set; }

        private void SyncConfig()
        {
            Configs = AlarmSystemConfigurationContext.GetDevices();
        }

        public AlarmRelaysListView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            Site = AlarmSystemConfigurationContext.Site;
            SyncConfig();
            
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < Configs.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                AlarmIoEntityGrid devGrid = new AlarmIoEntityGrid(Configs[i], AlarmSystemIoType.Relay);
                devGrid.SetTitle(Configs[i].Name);
                grid.Children.Add(devGrid, 0, i);
            }
            Content = grid;
        }
    }

}
