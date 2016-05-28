using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{
    public partial class SiteListView : ContentPage
    {

        public ISiteRepository Repository { get; set; }
        public List<sconnSite> Sites { get; set; }

        public ListView List { get; set; }

        private void LoadList()
        {
            Sites = Repository.GetAll().ToList();
        }

        public void AddSite_Clicked()
        {
            
        }

        public SiteListView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            
            Repository = new SiteSqlRepository();
            Sites = new List<sconnSite>();

            List = new ListView
            {
                RowHeight = 40,
               // BackgroundColor = Color.Aqua,
                ItemTemplate = new DataTemplate(typeof(SiteListCell))
            };
                 
            LoadList();
            
            List.ItemSelected += (sender, e) => {
                var site = (sconnSite)e.SelectedItem;
                AlarmSystemConfigurationContext.Site = site;
               var editPage = new SiteListPage {BindingContext = site };
                Navigation.PushModalAsync(editPage);
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

            //ToolbarItems.Add(new ToolbarItem
            //{
            //    Text = "",
            //    Icon = "Settings-50.png",      // "Add File-50.png",
            //    Order = ToolbarItemOrder.Primary,
            //    Priority = 1,
            //    Command = new Command(AddSite_Clicked)    // Navigation.PushAsync(new LaunchPage())
            //});


        }
    }
}
