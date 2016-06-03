using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace sconnRemMobile.View.AlarmSystem
{
	public partial class AlarmGlobalStatusView : ContentPage
	{
	    public sconnSite Site { get; set; }

	    public GlobalConfigService Service { get; set; }
	    public sconnGlobalConfig Config { get; set; }

	    public Button ArmControlButton { get; set; }

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

        }

		public AlarmGlobalStatusView ()
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
                Text = Config.Name,
                //FontSize = 2.0,
                FontFamily = "Copperplate"
            };



            /* Arm Grid */
            var ArmGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ArmGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            ArmGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            ArmGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });


            /* Arm icon */
            //var armImgButton = new Button
            //{
            //    Image = "eye1000.jpg",
            //    HorizontalOptions = LayoutOptions.FillAndExpand
            //};

            //        .MoveToRegion(
            //MapSpan.FromCenterAndRadius(
            //    new Position(37, -122), Distance.FromMiles(1)));

            var siteMap = new Map(
               MapSpan.FromCenterAndRadius(
                       new Position(49.6628,18.8528), Distance.FromMeters(500)))
                {
                IsShowingUser = true,
                HeightRequest = 400,
                WidthRequest = 400,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            siteMap.Pins.Add(new Pin
            {
                Label = "Techniserv",
                Position = new Position(49.662846,18.852836)
            });


            /* Arm button */
            ArmControlButton = new Button
            {
                Image = GetImagePathForArmStatus(),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ArmControlButton.Clicked += (sender, e) =>
            {
                Config.Armed = !Config.Armed;
                Service.Update(Config);
                UpdateArmStatus();
            };

            ArmGrid.Children.Add(siteMap, 0, 0);
            ArmGrid.Children.Add(ArmControlButton, 1, 0);



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

            var armedItem = new AlarmConfigurationListEntityGridItem("klucz1000.jpg", "Armed", Config.Armed.ToString());
            PropGrid.Children.Add(armedItem, 0, 0);

            var violationItem = new AlarmConfigurationListEntityGridItem("cctv.jpg", "Violation", Config.Violation.ToString());
            PropGrid.Children.Add(violationItem, 0, 1);
            
            var devicesItem = new AlarmConfigurationListEntityGridItem("strefa1000.jpg", "Devices", Config.Devices.ToString());
            PropGrid.Children.Add(devicesItem, 0, 2);

            var zonesItem = new AlarmConfigurationListEntityGridItem("strefy1-1000.jpg", "Zones", Config.Zones.ToString());
            PropGrid.Children.Add(zonesItem, 0, 3);

            var busComItem = new AlarmConfigurationListEntityGridItem("wtyczka1-1000.jpg", "RS485", Config.BusComEnabled.ToString());
            PropGrid.Children.Add(busComItem, 0, 4);

            var ethComItem = new AlarmConfigurationListEntityGridItem("rj1000.jpg", "TcpIp", Config.TcpIpComEnabled.ToString());
            PropGrid.Children.Add(ethComItem, 0, 5);

          

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) });

            grid.Children.Add(label,0,0);
            grid.Children.Add(ArmGrid,0,1);
            grid.Children.Add(PropGrid, 0, 2);

            Content = grid;
            
            
        }
	}
}
