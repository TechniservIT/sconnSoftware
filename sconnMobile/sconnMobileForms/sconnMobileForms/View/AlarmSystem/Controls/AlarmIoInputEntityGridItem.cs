using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{
 
    public class AlarmIoInputEntityGridItem : AlarmIoEntityGridItem
    {
        public sconnInput Input { get; set; }
        public Button IconButton { get; set; }
        
        public AlarmIoInputEntityGridItem()
        {
          
        }

        public AlarmIoInputEntityGridItem(sconnInput inputs) : this()
        {
            Input = inputs;
            Type = AlarmSystemIoType.Input;
            LoadUi();
        }

        private void LoadUi()
        {
            var box = new RoundedBox();
        

            var ioViewGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10)
            };
            ioViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ioViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            ioViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });


            var ioIconSwitchViewGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ioIconSwitchViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ioIconSwitchViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            
            var ArmControlButton = new Button
            {
                Image = AlarmIoEntityHelpers.AlarmIoEntityIconPathForType(Type),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ArmControlButton.Clicked += (sender, e) =>
            {
                //Config.Armed = !Config.Armed;
                //Service.Update(Config);
                //UpdateArmStatus();
            };

            
            ioIconSwitchViewGrid.Children.Add(ArmControlButton, 0, 0);
            

            var ioNameViewGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ioNameViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ioNameViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center,
                Text = Input.Name
            };
            //label.SetBinding(Label.TextProperty, "Name");
            ioNameViewGrid.Children.Add(label, 0, 0);


            ioViewGrid.Children.Add(ioIconSwitchViewGrid, 0, 0);
            ioViewGrid.Children.Add(ioNameViewGrid, 0, 1);
            

            Children.Add(ioViewGrid, 0, 0);


        }

    }

}
