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
