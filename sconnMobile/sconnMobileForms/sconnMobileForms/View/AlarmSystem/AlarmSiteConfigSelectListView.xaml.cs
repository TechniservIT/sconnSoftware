using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.SiteManagment;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{
	public partial class AlarmSiteConfigSelectListView : ContentPage
    { 
        public sconnSite Site { get; set; }
	    public List<AlarmSiteConfigurationEntity> Entities { get; set; }
        public ListView List { get; set; }
        
        public void AddSite_Clicked()
        {

        }

	    //public AlarmSiteConfigSelectListView()
	    //{
	            
	    //}

	    public AlarmSiteConfigSelectListView()  //(sconnSite site)
	    {
	        InitializeComponent();

	        Site = AlarmSystemConfigurationContext.Site;

	        NavigationPage.SetHasNavigationBar(this, true);

	        List = new ListView
	        {
	            RowHeight = 40,
	            ItemTemplate = new DataTemplate(typeof (AlarmSiteConfigurationEntityListItem))
	        };


            //List.ItemSelected += (sender, e) =>
            //{
            //    var site = (sconnSite) e.SelectedItem;
            //    var editPage = new SiteListPage {BindingContext = site};
            //    Navigation.PushModalAsync(editPage);
            //};

            Entities = new List<AlarmSiteConfigurationEntity>()
            {
                new AlarmSiteConfigurationEntity("test","aku1000.jpg",CommandConfigType.NET_PACKET_TYPE_AUTH),
                new AlarmSiteConfigurationEntity("test1","aku1000.jpg",CommandConfigType.NET_PACKET_TYPE_AUTH),
                new AlarmSiteConfigurationEntity("test2","aku1000.jpg",CommandConfigType.NET_PACKET_TYPE_AUTH),
                new AlarmSiteConfigurationEntity("testgdsgfd14","aku1000.jpg",CommandConfigType.NET_PACKET_TYPE_AUTH)
            };



            List.ItemsSource = Entities;

	        var layout = new StackLayout();

	        layout.Children.Add(List);
	        layout.VerticalOptions = LayoutOptions.FillAndExpand;
	        Content = layout;

	    }

	}
    }
