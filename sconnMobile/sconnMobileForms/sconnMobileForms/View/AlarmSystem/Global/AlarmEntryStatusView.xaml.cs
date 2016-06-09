using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace sconnMobileForms.View.AlarmSystem.Global
{

    public partial class AlarmEntryStatusView : ContentPage
    {
        public sconnSite Site { get; set; }

        public GlobalConfigService Service { get; set; }
        public sconnGlobalConfig Config { get; set; }

        public Button ArmControlButton { get; set; }
        public Switch ArmControlSwitch { get; set; }

        private void SyncConfig()
        {
            Config = Service.Get();

        }

        private string GetImagePathForArmStatus()
        {
            try
            {
                bool armed = Config.Armed;
                if (armed)
                {
                    return "zazb-1000.jpg";
                }
                return "rozb-1000.jpg";
            }
            catch (Exception)
            {

                return null;
            }
        }

        private void UpdateArmStatus()
        {
            SyncConfig();
            ArmControlButton.Image = GetImagePathForArmStatus();
            ArmControlSwitch.IsToggled = Config.Armed;
        }

        public AlarmEntryStatusView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            Site = AlarmSystemConfigurationContext.Site;
            Service = AlarmSystemConfigurationContext.GetGlobalConfigService();

            SyncConfig();

            /* Name */
            var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = Config.Name,
                FontFamily = "Copperplate"
            };

            /* Arm Grid */
            var armGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            armGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            armGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            armGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });


            //map item grid
            var siteMap = new Map(
               MapSpan.FromCenterAndRadius(
                       new Position(49.6628, 18.8528), Distance.FromMeters(500)))
            {
                IsShowingUser = true,
                HeightRequest = 400,
                WidthRequest = 400,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            siteMap.Pins.Add(new Pin
            {
                Label = "Techniserv",
                Position = new Position(49.662846, 18.852836)
            });


            /* Arm item grid */
            var armControlGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            armControlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            armControlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.7, GridUnitType.Star) });
            armControlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });

            ArmControlButton = new Button
            {
                Image = GetImagePathForArmStatus(),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ArmControlButton.Clicked += (sender, e) =>
            {
                Service.Update(Config);
                UpdateArmStatus();
            };

            ArmControlSwitch = new Switch()
            {
                Margin = new Thickness(2, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            ArmControlSwitch.SetBinding(Switch.IsToggledProperty, "Armed", BindingMode.TwoWay);
            ArmControlSwitch.IsToggled = Config.Armed;

            ArmControlSwitch.Toggled += (sender, e) =>
            {
                Config.Armed = ArmControlSwitch.IsToggled;
                Service.Update(Config);
                UpdateArmStatus();
            };

            armControlGrid.Children.Add(ArmControlButton, 0, 0);
            armControlGrid.Children.Add(ArmControlSwitch, 0, 1);


            //arm items to grid
            armGrid.Children.Add(siteMap, 0, 0);
            armGrid.Children.Add(armControlGrid, 1, 0);



            /* Details item grid */
            var showDetailsGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            showDetailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1,GridUnitType.Star) });
            showDetailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) });
            showDetailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });
            showDetailsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


            var showDetailsIcon = new Button
            {
                Image = "config1000 (2).jpg",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var showDetailsLabel = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Details",
                FontFamily = "Copperplate"
            };

            var showDetailsButton = new Button
            {
                Image = "dalej500x500.jpg",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            showDetailsButton.Clicked += (sender, e) =>
            {
                var site = (sconnSite)Site;
                AlarmSystemConfigurationContext.Site = site;
                var editPage = new AlarmSiteConfigSelectListView() { BindingContext = AlarmSystemConfigurationContext.Site };
                Navigation.PushAsync(editPage);
            };

            showDetailsGrid.Children.Add(showDetailsIcon, 0, 0);
            showDetailsGrid.Children.Add(showDetailsLabel, 1, 0);
            showDetailsGrid.Children.Add(showDetailsButton, 2, 0);


            /* Properties Grid */
            var PropGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //Padding = new Thickness(0, 6, 0, 6)
            };
            PropGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });


            /* Info labels */

            var armedItem = new AlarmConfigurationListEntityGridItem("klucz1000.jpg", "Armed : ", Config.Armed.ToString());
            PropGrid.Children.Add(armedItem, 0, 0);

            var violationItem = new AlarmConfigurationListEntityGridItem("cctv.jpg", "Violation : ", Config.Violation.ToString());
            PropGrid.Children.Add(violationItem, 0, 1);

            var devicesItem = new AlarmConfigurationListEntityGridItem("strefa1000.jpg", "Devices : ", Config.Devices.ToString());
            PropGrid.Children.Add(devicesItem, 0, 2);

            var zonesItem = new AlarmConfigurationListEntityGridItem("strefy1-1000.jpg", "Zones : ", Config.Zones.ToString());
            PropGrid.Children.Add(zonesItem, 0, 3);

            var busComItem = new AlarmConfigurationListEntityGridItem("wtyczka1-1000.jpg", "RS485 : ", Config.BusComEnabled.ToString());
            PropGrid.Children.Add(busComItem, 0, 4);

            var ethComItem = new AlarmConfigurationListEntityGridItem("rj1000.jpg", "TcpIp : ", Config.TcpIpComEnabled.ToString());
            PropGrid.Children.Add(ethComItem, 0, 5);

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(4, 4, 4, 4)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });

            grid.Children.Add(label, 0, 0);
            grid.Children.Add(armGrid, 0, 1);
            grid.Children.Add(showDetailsGrid, 0, 2);
            grid.Children.Add(PropGrid, 0, 3);

            Content = grid;


        }
    }


}
