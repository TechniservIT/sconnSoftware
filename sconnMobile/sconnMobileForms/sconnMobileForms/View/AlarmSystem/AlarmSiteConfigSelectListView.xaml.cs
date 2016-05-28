using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnMobileForms.View.SiteManagment;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{
	public partial class AlarmSiteConfigSelectListView : ContentPage
    { 
        public sconnSite Site { get; set; }
        public ListView List { get; set; }
        
        public void AddSite_Clicked()
        {

        }

	    public AlarmSiteConfigSelectListView(sconnSite site)
	    {
	        InitializeComponent();

	        NavigationPage.SetHasNavigationBar(this, true);

	        Site = site;

	        List = new ListView
	        {
	            RowHeight = 40,
	            ItemTemplate = new DataTemplate(typeof (SiteListCell))
	        };

	        LoadList();

	        List.ItemSelected += (sender, e) =>
	        {
	            var site = (sconnSite) e.SelectedItem;
	            var editPage = new SiteListPage {BindingContext = site};
	            Navigation.PushModalAsync(editPage);
	        };

	        List.ItemsSource = Sites;

	        var layout = new StackLayout();

	        layout.Children.Add(List);
	        layout.VerticalOptions = LayoutOptions.FillAndExpand;
	        Content = layout;

	    }

	}
    }
