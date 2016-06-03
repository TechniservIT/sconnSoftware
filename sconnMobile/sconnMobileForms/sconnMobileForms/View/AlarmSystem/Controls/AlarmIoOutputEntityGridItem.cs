using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.Service.AlarmSystem.Io;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{
   
    public class AlarmIoOutputEntityGridItem : AlarmIoEntityGridItem
    {
        public sconnOutput Output { get; set; }
        public Button IconButton { get; set; }
        public IAlarmIoConfigService Service { get; set; }

        public AlarmIoOutputEntityGridItem()
        {

        }

        public AlarmIoOutputEntityGridItem(IAlarmIoConfigService service, sconnOutput output) : this()
        {
            Output = output;
            Service = service;
            Type = AlarmSystemIoType.Output;
            LoadUi();
        }

        private void LoadUi()
        {
            var ioViewGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ioViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ioViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            ioViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });


            var ioIconSwitchViewGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ioIconSwitchViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            ioIconSwitchViewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            ioIconSwitchViewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var ArmControlButton = new Button
            {
                Image = AlarmIoEntityHelpers.AlarmIoEntityIconPathForType(Type),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ArmControlButton.Clicked += (sender, e) =>
            {
                Output.Value = !Output.Value;
                AlarmSystemConfigurationContext.SaveOutputGeneric(Output);
            };

            var armControlSwitch = new Switch()
            {
                Margin = new Thickness(2, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            //  armControlSwitch.SetBinding(Switch.IsToggledProperty, "Value");
            armControlSwitch.IsToggled = Output.Value;
            armControlSwitch.Toggled += (sender, e) =>
            {
                Output.Value = !Output.Value;
                AlarmSystemConfigurationContext.SaveOutputGeneric(Output);
            };

            ioIconSwitchViewGrid.Children.Add(ArmControlButton, 0, 0);
            ioIconSwitchViewGrid.Children.Add(armControlSwitch, 1, 0);
            

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
                Text = Output.Name
            };
            //label.SetBinding(Label.TextProperty, "Name");
            ioNameViewGrid.Children.Add(label, 0, 0);


            ioViewGrid.Children.Add(ioIconSwitchViewGrid, 0, 0);
            ioViewGrid.Children.Add(ioNameViewGrid, 0, 1);


            Children.Add(ioViewGrid, 0, 0);
        }

    }
}
