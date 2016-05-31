using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{
    public partial class SiteListView : ContentPage
    {

        public ISiteRepository Repository { get; set; }
        public ObservableCollection<sconnSite> Sites { get; set; }

        public ListView List { get; set; }

        private void LoadList()
        {
            List.ItemsSource = null;
            Sites = Repository.GetAll(); // new ObservableCollection<sconnSite>);
            List.ItemsSource = Sites;
        }

        public void AddSite_Clicked()
        {
            sconnSite site  = new sconnSite();
            AlarmSystemConfigurationContext.Site = site;
            var editPage = new SiteListPage { BindingContext = AlarmSystemConfigurationContext.Site };
            Navigation.PushAsync(editPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadList();
        }

        public SiteListView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            AlarmSystemConfigurationContext.Repository = new SiteSqlRepository();
            Repository = AlarmSystemConfigurationContext.Repository;

            Sites = new ObservableCollection<sconnSite>();  // new List<sconnSite>();

            List = new ListView
            {
                RowHeight = 40,
               // BackgroundColor = Color.Aqua,
                ItemTemplate = new DataTemplate(typeof(SiteListCell))
            };
                 
            LoadList();
            
            List.ItemTapped += (sender, e) => {
                var site = (sconnSite)e.Item;
                AlarmSystemConfigurationContext.Site = site;

                var editPage = new AlarmSiteConfigSelectListView() { BindingContext = AlarmSystemConfigurationContext.Site };
                Navigation.PushAsync(editPage);
              
            };

            List.ItemsSource = Sites;

            var layout = new StackLayout();

            layout.Children.Add(List);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;
            
           
            ToolbarItems.Add(new ToolbarItem {
	            Text = "",
                Icon = "Plus Math-40.png",      // "Add File-50.png",
                Order = ToolbarItemOrder.Primary,
              //  Priority = 2,
	            Command = new Command( AddSite_Clicked)    // Navigation.PushAsync(new LaunchPage())
            });
            

        }
    }
}
