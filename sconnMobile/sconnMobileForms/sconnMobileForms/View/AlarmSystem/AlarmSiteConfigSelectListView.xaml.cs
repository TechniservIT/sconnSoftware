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
    public enum AlarmSystemConfigurationViewType
    {
        Global,
        Zones,
        Inputs,
        Outputs,
        Relays,
        Power,
        Gsm,
        Users
    }

	public partial class AlarmSiteConfigSelectListView : ContentPage
    { 
        public sconnSite Site { get; set; }
	    public List<AlarmSiteConfigurationEntity> Entities { get; set; }
        public ListView List { get; set; }


	    private void ShowEntityConfigView(AlarmSiteConfigurationEntity entity)
	    {
            ContentPage configView = null;
            if (entity.Type == AlarmSystemConfigurationViewType.Zones)
            {
                configView = new AlarmZonesListView();
            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Inputs)
            {
                configView = new AlarmInputsListView();
            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Outputs)
            {
                configView = new AlarmOutputsListView();
            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Relays)
            {
                configView = new AlarmRelaysListView();
            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Global)
            {
                configView = new AlarmGlobalStatusView();
            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Power)
            {

            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Gsm)
            {
                configView = new AlarmGsmRcptListView();
            }
            else if (entity.Type == AlarmSystemConfigurationViewType.Users)
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
                new AlarmSiteConfigurationEntity("Global","glob1000.jpg",AlarmSystemConfigurationViewType.Global),
                new AlarmSiteConfigurationEntity("Zones","strefy2-1000.jpg",AlarmSystemConfigurationViewType.Zones),
                new AlarmSiteConfigurationEntity("Inputs","cctv.jpg",AlarmSystemConfigurationViewType.Inputs),
                new AlarmSiteConfigurationEntity("Outputs","elektro1000.jpg",AlarmSystemConfigurationViewType.Outputs),
                 new AlarmSiteConfigurationEntity("Relays","przekaznik1000.jpg",AlarmSystemConfigurationViewType.Relays),
                new AlarmSiteConfigurationEntity("Power","aku1000.jpg",AlarmSystemConfigurationViewType.Power),
                 new AlarmSiteConfigurationEntity("Gsm","tel1000.jpg",AlarmSystemConfigurationViewType.Gsm)
            };
            
            List.ItemsSource = Entities;
	        var layout = new StackLayout();

	        layout.Children.Add(List);
	        layout.VerticalOptions = LayoutOptions.FillAndExpand;
	        Content = layout;

	    }

	}
    }
