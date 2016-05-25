using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{

    public class SiteListCell : ViewCell
    {
        public SiteListCell()
        {
            var label = new Label
            {
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            
            label.SetBinding(Label.TextProperty, "siteName");

            var saveButton = new Button { Image = "add44.png"};
            saveButton.Clicked += (sender, e) => {
                
            };


            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { label, saveButton }
            };


            View = layout;
        }
    }


}
