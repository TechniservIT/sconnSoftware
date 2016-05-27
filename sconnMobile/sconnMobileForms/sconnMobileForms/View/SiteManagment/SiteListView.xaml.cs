using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
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
                BackgroundColor = Color.Aqua,
                ItemTemplate = new DataTemplate(typeof(SiteListCell))
            };
                 
            LoadList();
            
            List.ItemSelected += (sender, e) => {
                var site = (sconnSite)e.SelectedItem;
                var editPage = new SiteListPage {BindingContext = site };
                Navigation.PushModalAsync(editPage);
            };

            List.ItemsSource = Sites;

            var layout = new StackLayout();

            layout.Children.Add(List);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;


            ToolbarItem tbi = null;
            if (Device.OS == TargetPlatform.iOS)
            {
                tbi = new ToolbarItem("+", null, () => {
                    var todoItem = new sconnSite();
                    var todoPage = new SiteListPage();
                    todoPage.BindingContext = todoItem;
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }
            if (Device.OS == TargetPlatform.Android)
            { // BUG: Android doesn't support the icon being null
                tbi = new ToolbarItem("+", "plus", () => {
                    var todoItem = new sconnSite();
                    var todoPage = new SiteListPage();
                    todoPage.BindingContext = todoItem;
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }

            //if (Device.OS == TargetPlatform.WinPhone)
            //{
            //    tbi = new ToolbarItem("Add", "add.png", () => {
            //        var todoItem = new sconnSite();
            //        var todoPage = new SiteListPage();
            //        todoPage.BindingContext = todoItem;
            //        Navigation.PushAsync(todoPage);
            //    }, 0, 0);
            //}

            //if (tbi != null) ToolbarItems.Add(tbi);

            //var embeddedImage = new Image { Aspect = Aspect.AspectFit };
            //embeddedImage.Source = ImageSource.FromResource("add.jpg");
            
           


            ToolbarItems.Add(new ToolbarItem {
	            Text = "",
                Icon = "Add File-50.png",
                Order = ToolbarItemOrder.Primary,
	            Command = new Command( AddSite_Clicked)    // Navigation.PushAsync(new LaunchPage())
            });

        }
    }
}
