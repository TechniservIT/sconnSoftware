using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.SiteManagment;
using sconnRemMobile.View.AlarmSystem;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{
	public partial class AlarmSiteConfigSelectListView : ContentPage
    { 
        public sconnSite Site { get; set; }
	    public List<AlarmSiteConfigurationEntity> Entities { get; set; }
        public ListView List { get; set; }


	    private void ShowEntityConfigView(AlarmSiteConfigurationEntity entity)
	    {
            ContentPage configView = null;
            if (entity.Type == CommandConfigType.NET_PACKET_TYPE_AUTH)
            {

            }
            else if (entity.Type == CommandConfigType.NET_PACKET_TYPE_DEVAUTHCFG)
            {

            }
            else if (entity.Type == CommandConfigType.NET_PACKET_TYPE_DEVCFG)
            {

            }
            else if (entity.Type == CommandConfigType.NET_PACKET_TYPE_AUTH)
            {

            }
            else if (entity.Type == CommandConfigType.NET_PACKET_TYPE_DEVNAMECFG)
            {

            }
            else if (entity.Type == CommandConfigType.NET_PACKET_TYPE_GCFG)
            {
                configView = new AlarmGlobalStatusView();
            }
            else if (entity.Type == CommandConfigType.NET_PACKET_TYPE_DEVNETCFG)
            {

            }

	        if (configView != null)
	        {
                Navigation.PushAsync(configView);
            }
           
        }

	    public AlarmSiteConfigSelectListView()
	    {
	        InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);

            Site = AlarmSystemConfigurationContext.Site;

	        List = new ListView
	        {
	            RowHeight = 40,
	            ItemTemplate = new DataTemplate(typeof (AlarmSiteConfigurationEntityListItem))
	        };


            List.ItemTapped += (sender, e) =>
            {
                AlarmSiteConfigurationEntity entity = (AlarmSiteConfigurationEntity)e.Item;
                ShowEntityConfigView(entity);
            };

            Entities = new List<AlarmSiteConfigurationEntity>()
            {
                new AlarmSiteConfigurationEntity("Global","glob1000.jpg",CommandConfigType.NET_PACKET_TYPE_GCFG),
                new AlarmSiteConfigurationEntity("Zones","strefy2-1000.jpg",CommandConfigType.NET_PACKET_TYPE_ZONECFG),
                new AlarmSiteConfigurationEntity("Inputs","cctv.jpg",CommandConfigType.NET_PACKET_TYPE_DEVCFG),
                new AlarmSiteConfigurationEntity("Outputs","elektro1000.jpg",CommandConfigType.NET_PACKET_TYPE_DEVCFG)
            };
            
            List.ItemsSource = Entities;
	        var layout = new StackLayout();

	        layout.Children.Add(List);
	        layout.VerticalOptions = LayoutOptions.FillAndExpand;
	        Content = layout;

	    }

	}
    }
