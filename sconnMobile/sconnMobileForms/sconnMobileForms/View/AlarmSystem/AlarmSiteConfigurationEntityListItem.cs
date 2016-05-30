using System;
using System.Collections.Generic;
using System.Text;
using iotDbConnector.DAL;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{

    public class AlarmSiteConfigurationEntityListItem : ViewCell
    {
       

        public AlarmSiteConfigurationEntityListItem()
        {
             var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center
            };
            label.SetBinding(Label.TextProperty, "Name");
          
            var saveButton = new Button
            {
                Image = "config2.png",
                HorizontalOptions = LayoutOptions.End
            };
            saveButton.SetBinding(Button.ImageProperty, "Uri");



            saveButton.Clicked += (sender, e) =>
            {

            };


            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 6, 8, 6)
            };


            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });  //GridLength.Auto});
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.9, GridUnitType.Star) });

            grid.Children.Add(saveButton, 0, 0);
            grid.Children.Add(label, 1, 0);
            

            View = grid;

        }
    }


}
