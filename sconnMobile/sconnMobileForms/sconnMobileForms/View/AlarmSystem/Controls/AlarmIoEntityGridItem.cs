using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{

    public class AlarmIoEntityGridItem : Grid
    {
        public string Description { get; set; }
        public string IconUri { get; set; }
        public AlarmSystemIoType Type { get; set; }

        public AlarmIoEntityGridItem()
        {
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Padding = new Thickness(5, 5, 5, 5);
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        public bool Get()
        {
            return false;
        }

        public bool Set()
        {
            return false;
        }

        public void Highlight()
        {
            
        }
    }

 

}
