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
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center
            };

            label.SetBinding(Label.TextProperty, "siteName");


            var saveButton = new Button
            {
                Image = "config2.png",
                HorizontalOptions = LayoutOptions.End
            };

            saveButton.Clicked += (sender, e) =>
            {

            };


            var grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 6, 8, 6)
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(0.8, GridUnitType.Star)});
            grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(0.2, GridUnitType.Star)});  //GridLength.Auto});

                grid.Children.Add(label);
                grid.Children.Add(saveButton, 1, 0);
            
                View = grid;

            }
        }

}
