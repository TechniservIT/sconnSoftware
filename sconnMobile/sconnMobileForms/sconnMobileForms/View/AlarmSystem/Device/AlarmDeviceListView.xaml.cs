using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem;
using sconnMobileForms.View.AlarmSystem.Device;
using sconnMobileForms.View.AlarmSystem.Zone;
using Xamarin.Forms;

namespace sconnRemMobile.View.AlarmSystem
{

    public partial class AlarmDeviceListView : ContentPage
    {
        public AlarmDevicesConfigService Repository { get; set; }
        public ObservableCollection<sconnDevice> Devices { get; set; }

        public ListView List { get; set; }

        private void LoadList()
        {
            List.ItemsSource = null;
            Devices = new ObservableCollection<sconnDevice>(Repository.GetAll());
            List.ItemsSource = Devices;
        }

        public void AddZone_Clicked()
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadList();
        }

        public AlarmDeviceListView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);

            Repository = AlarmSystemConfigurationContext.DevicesService;

            Devices = new ObservableCollection<sconnDevice>();

            List = new ListView
            {
                RowHeight = 40,
                ItemTemplate = new DataTemplate(typeof(AlarmDeviceListItem))
            };

            LoadList();

            List.ItemTapped += (sender, e) => {
                sconnDevice device = (sconnDevice)e.Item;
                var editPage = new AlarmDeviceEditView{ BindingContext = device };
                Navigation.PushAsync(editPage);
            };

            List.ItemsSource = Devices;

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
