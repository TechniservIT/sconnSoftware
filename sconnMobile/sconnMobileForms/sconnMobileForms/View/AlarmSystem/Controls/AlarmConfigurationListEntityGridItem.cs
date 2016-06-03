using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{

    public class AlarmConfigurationListEntityGridItem : Grid
    {
        public Button IconButton { get; set; }
        public Label DescriptionLabel { get; set; }
        public Label ContentLabel { get; set; }

        public AlarmConfigurationListEntityGridItem(string path, string desc, string content)
        {

            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
                //Padding = new Thickness(0, 6, 0, 6)
       
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) });

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            IconButton = new Button
            {
                Image = path,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Children.Add(IconButton, 0, 0);

            DescriptionLabel = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                Text = desc
            };
            Children.Add(DescriptionLabel, 1, 0);

            ContentLabel = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                Text = content
            };
            Children.Add(ContentLabel, 2, 0);



        }


    }

}
