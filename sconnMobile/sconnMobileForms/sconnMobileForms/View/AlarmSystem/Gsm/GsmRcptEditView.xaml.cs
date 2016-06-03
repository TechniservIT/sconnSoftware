using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Gsm
{


    public partial class GsmRcptEditView : ContentPage
    {

        Dictionary<string, GsmMessagingLevel> gsmLevels = new Dictionary<string, GsmMessagingLevel>
        {
            {GsmMessagingLevel.All.ToString(), GsmMessagingLevel.All },
            {GsmMessagingLevel.ArmChange.ToString(), GsmMessagingLevel.ArmChange },
            {GsmMessagingLevel.ConfigChange.ToString(), GsmMessagingLevel.ConfigChange },
            {GsmMessagingLevel.IoControl.ToString(), GsmMessagingLevel.IoControl },
            {GsmMessagingLevel.PowerChange.ToString(), GsmMessagingLevel.PowerChange },
            {GsmMessagingLevel.UserChange.ToString(), GsmMessagingLevel.UserChange },
            {GsmMessagingLevel.Violations.ToString(), GsmMessagingLevel.Violations }
        };

        public sconnGsmRcpt Config { get; set; }

        private void SaveSite()
        {
            //load data - binding

            //validate

            //save
        }


        public GsmRcptEditView()
        {
            this.SetBinding(ContentPage.TitleProperty, "Name");

            NavigationPage.SetHasNavigationBar(this, true);

            var nameLabel = new Label { Text = "Name :  " };
            var nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "Name");

            //bool
            var enabledLabel = new Label { Text = "Enabled :" };
            var enabledEntry = new Switch();
            enabledEntry.SetBinding(Switch.IsEnabledProperty, "Enabled");


            var ccLabel = new Label { Text = "CountryCode  : " };
            var ccEntry = new Entry();
            ccEntry.SetBinding(Entry.TextProperty, "CountryCode");

            var addressLabel = new Label { Text = "NumberE164  : " };
            var addressEntry = new Entry();
            addressEntry.SetBinding(Entry.TextProperty, "NumberE164");

            //enum
            var msgLevelLabel = new Label { Text = "MessageLevel : " };
            Picker picker = new Picker
            {
                Title = "MessageLevel",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            picker.SetBinding(Picker.SelectedIndexProperty, "MessageLevel");

            foreach (var level in gsmLevels.Keys)
            {
                picker.Items.Add(level);
            }

            //picker.SelectedIndexChanged += (sender, args) =>
            //{
            //    if (picker.SelectedIndex == -1)
            //    {
            //        boxView.Color = Color.Default;
            //    }
            //    else
            //    {
            //        string colorName = picker.Items[picker.SelectedIndex];
            //        boxView.Color = nameToColor[colorName];
            //    }
            //};


            //var msgLevelEntry = new Entry();



            var saveButton = new Button { Text = "Save" };
            saveButton.Clicked += (sender, e) => {
                //TODO
                Navigation.PopAsync();
            };


            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(20),
                Children = {
                    nameLabel, nameEntry,
                    enabledLabel, enabledEntry,
                    ccLabel, ccEntry,
                    addressLabel,addressEntry,
                    msgLevelLabel,picker
                }
            };

        }

    }


}
