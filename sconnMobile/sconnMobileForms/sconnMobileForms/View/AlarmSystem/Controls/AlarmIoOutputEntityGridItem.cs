using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{
   
    public class AlarmIoOutputEntityGridItem : AlarmIoEntityGridItem
    {
        public sconnOutput Output { get; set; }
        public Button IconButton { get; set; }

        public AlarmIoOutputEntityGridItem()
        {

        }

        public AlarmIoOutputEntityGridItem(sconnOutput output) : this()
        {
            Type = AlarmSystemIoType.Output;
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
