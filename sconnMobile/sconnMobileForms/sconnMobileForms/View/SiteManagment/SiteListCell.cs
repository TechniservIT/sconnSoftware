using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{

    public class SiteListCell : ViewCell
    {
        public SiteListCell()
        {
            var label = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            label.SetBinding(Label.TextProperty, "siteName");
            
            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { label }
            };

            View = layout;
        }
    }


}
