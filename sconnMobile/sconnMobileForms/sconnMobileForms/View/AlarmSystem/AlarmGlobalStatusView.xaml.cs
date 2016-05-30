using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using Xamarin.Forms;

namespace sconnRemMobile.View.AlarmSystem
{
	public partial class AlarmGlobalStatusView : ContentPage
	{
	    public sconnSite Site { get; set; }

		public AlarmGlobalStatusView ()
		{

            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);

            Site = AlarmSystemConfigurationContext.Site;

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center,
                Text = "Global"
            };

          //  label.SetBinding(Label.TextProperty, "siteName");

            var layout = new StackLayout();

            layout.Children.Add(label);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;
            
        }
	}
}
