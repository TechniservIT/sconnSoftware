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
            IconButton = new Button
            {
                Image = AlarmIoEntityHelpers.AlarmIoEntityIconPathForType(Type),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            IconButton.Clicked += (sender, e) =>
            {
                Service.Toggle();
            };
            Children.Add(IconButton, 0, 0);
        }

    }
}
