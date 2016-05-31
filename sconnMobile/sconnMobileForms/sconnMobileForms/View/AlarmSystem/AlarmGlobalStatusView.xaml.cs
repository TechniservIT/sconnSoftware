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
                YAlign = TextAlignment.Center,
                
                Text = Config.Name
            };



            /* Arm Grid */
            var ArmGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 6, 8, 6)
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

            var siteMap = new Map(
               MapSpan.FromCenterAndRadius(
                       new Position(37, -122), Distance.FromMiles(0.3)))
                {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

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
                Padding = new Thickness(0, 6, 8, 6)
            };
            PropGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
            PropGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });


            /* Info labels */
            var zonesLabel = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center,
                Text = AlarmSystemConfigurationContext.Site.siteCfg.ZoneNumber.ToString()
            };
            PropGrid.Children.Add(zonesLabel, 0, 0);



            //Global grid

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 6, 8, 6)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) });

            grid.Children.Add(label,0,0);
            grid.Children.Add(ArmGrid,0,1);
            grid.Children.Add(PropGrid, 0, 2);

            Content = grid;

            //var layout = new StackLayout();
            //layout.Children.Add(label);
            //layout.VerticalOptions = LayoutOptions.FillAndExpand;
            //Content = layout;
            
        }
	}
}
