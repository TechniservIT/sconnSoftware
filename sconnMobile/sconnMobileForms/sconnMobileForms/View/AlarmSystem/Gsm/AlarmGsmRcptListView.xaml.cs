using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem;
using sconnMobileForms.View.AlarmSystem.Gsm;
using sconnMobileForms.View.AlarmSystem.Zone;
using Xamarin.Forms;

namespace sconnRemMobile.View.AlarmSystem
{

    public partial class AlarmGsmRcptListView : ContentPage
    {
        public GsmConfigurationService Repository { get; set; }
        public ObservableCollection<sconnGsmRcpt> Rcpts { get; set; }

        public ListView List { get; set; }

        private void LoadList()
        {
            List.ItemsSource = null;
            Rcpts = new ObservableCollection<sconnGsmRcpt>(Repository.GetAll());
            List.ItemsSource = Rcpts;
        }

        public void AddZone_Clicked()
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadList();
        }

        public AlarmGsmRcptListView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);

            Repository = AlarmSystemConfigurationContext.GsmService;

            Rcpts = new ObservableCollection<sconnGsmRcpt>();

            List = new ListView
            {
                RowHeight = 40,
                ItemTemplate = new DataTemplate(typeof(GsmRcptListItem))
            };

            LoadList();

            List.ItemTapped += (sender, e) => {
                sconnGsmRcpt rcpt = (sconnGsmRcpt)e.Item;
                var editPage = new GsmRcptEditView{ BindingContext = rcpt };
                Navigation.PushAsync(editPage);
            };

            List.ItemsSource = Rcpts;

            var layout = new StackLayout();

            layout.Children.Add(List);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "",
                Icon = "Plus Math-40.png",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(AddZone_Clicked)
            });


        }



    }
}
