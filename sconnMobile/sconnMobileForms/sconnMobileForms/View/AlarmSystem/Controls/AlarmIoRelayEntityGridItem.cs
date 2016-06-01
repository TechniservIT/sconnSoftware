using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{

    public class AlarmIoRelayEntityGridItem : AlarmIoEntityGridItem
    {
     
        public sconnRelay Relay { get; set; }
        public Button IconButton { get; set; }

        public AlarmIoRelayEntityGridItem()
        {

        }

        public AlarmIoRelayEntityGridItem(sconnRelay relay) : this()
        {
            Relay = relay;
            Type = AlarmSystemIoType.Relay;
            LoadUi();
        }

        private void LoadUi()
        {
            IconButton = new Button
            {
                Image = AlarmIoEntityHelpers.AlarmIoEntityIconPathForType(Type),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            IconButton.Clicked += (sender, e) =>
            {

            };
            Children.Add(IconButton, 0, 0);
        }
    }

}
