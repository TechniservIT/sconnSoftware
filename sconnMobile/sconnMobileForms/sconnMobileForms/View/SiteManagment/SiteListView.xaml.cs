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

        public SiteListView()
        {

            Repository = new SiteSqlRepository();
            Sites = new List<sconnSite>();
            List = new ListView();

            List = new ListView
            {
                RowHeight = 40,
                ItemTemplate = new DataTemplate(typeof(SiteListCell))
            };


            List.ItemSelected += (sender, e) => {
                var todoItem = (sconnSite	)e.SelectedItem;
                var todoPage = new TodoItemPage();
                todoPage.BindingContext = todoItem;
                Navigation.PushAsync(todoPage);
            };

            var layout = new StackLayout();
            if (Device.OS == TargetPlatform.WinPhone)
            { // WinPhone doesn't have the title showing
                layout.Children.Add(new Label
                {
                    Text = "Todo",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    FontAttributes = FontAttributes.Bold
                });
            }
            layout.Children.Add(listView);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;


            ToolbarItem tbi = null;
            if (Device.OS == TargetPlatform.iOS)
            {
                tbi = new ToolbarItem("+", null, () => {
                    var todoItem = new TodoItem();
                    var todoPage = new TodoItemPage();
                    todoPage.BindingContext = todoItem;
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }
            if (Device.OS == TargetPlatform.Android)
            { // BUG: Android doesn't support the icon being null
                tbi = new ToolbarItem("+", "plus", () => {
                    var todoItem = new TodoItem();
                    var todoPage = new TodoItemPage();
                    todoPage.BindingContext = todoItem;
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }

            if (Device.OS == TargetPlatform.WinPhone)
            {
                tbi = new ToolbarItem("Add", "add.png", () => {
                    var todoItem = new TodoItem();
                    var todoPage = new TodoItemPage();
                    todoPage.BindingContext = todoItem;
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }

            ToolbarItems.Add(tbi);

            if (Device.OS == TargetPlatform.iOS)
            {
                var tbi2 = new ToolbarItem("?", null, () => {
                    var todos = App.Database.GetItemsNotDone();
                    var tospeak = "";
                    foreach (var t in todos)
                        tospeak += t.Name + " ";
                    if (tospeak == "")
                        tospeak = "there are no tasks to do";

                    DependencyService.Get<ITextToSpeech>().Speak(tospeak);
                }, 0, 0);
                ToolbarItems.Add(tbi2);
            }


            LoadList();

            InitializeComponent();

        }
    }
}
