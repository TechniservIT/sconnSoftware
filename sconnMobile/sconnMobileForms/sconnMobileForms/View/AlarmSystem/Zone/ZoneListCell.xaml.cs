using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Zone
{
	public partial class ZoneListCell : ViewCell
    {
		public ZoneListCell ()
		{

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center
            };

            label.SetBinding(Label.TextProperty, "Name");


            var iconButton = new Button
            {
                Image = "strefy2-1000.jpg",
                HorizontalOptions = LayoutOptions.End
            };
            
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 6, 8, 6)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.9, GridUnitType.Star) });  //GridLength.Auto});

            grid.Children.Add(iconButton, 0, 0);
            grid.Children.Add(label,1,0);


            View = grid;

        }



    }
}
