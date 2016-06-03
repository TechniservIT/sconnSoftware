using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem;
using sconnMobileForms.View.AlarmSystem.Zone;
using sconnMobileForms.View.SiteManagment;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnRemMobile.View.AlarmSystem
{


    public partial class AlarmZonesListView : ContentPage
    {
        public ZoneConfigurationService Repository { get; set; }
        public ObservableCollection<sconnAlarmZone> Zones { get; set; }

        public ListView List { get; set; }

        private void LoadList()
        {
            List.ItemsSource = null;
            Zones = new ObservableCollection<sconnAlarmZone>(Repository.GetAll()); 
            List.ItemsSource = Zones;
        }

        public void AddZone_Clicked()
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadList();
        }

        public AlarmZonesListView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);

            Repository = AlarmSystemConfigurationContext.ZoneService;

            Zones = new ObservableCollection<sconnAlarmZone>();

            List = new ListView
            {
                RowHeight = 40,
                ItemTemplate = new DataTemplate(typeof(ZoneListCell))
            };

            LoadList();

            List.ItemTapped += (sender, e) => {
                sconnAlarmZone zone = (sconnAlarmZone)e.Item;
                var editPage = new AlarmZoneEditView{ BindingContext = zone };
                Navigation.PushAsync(editPage);
            };

            List.ItemsSource = Zones;

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
