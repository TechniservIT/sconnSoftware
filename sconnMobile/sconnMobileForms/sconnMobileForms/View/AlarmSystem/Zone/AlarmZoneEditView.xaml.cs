using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.sconn;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem
{


    public partial  class AlarmZoneEditView : ContentPage
    {
        public sconnAlarmZone Config { get; set; }
        
        private void SaveSite()
        {
            //load data - binding

            //validate
            
            //save
        }
        

        public AlarmZoneEditView()
        {
            this.SetBinding(ContentPage.TitleProperty, "Name");
            
            NavigationPage.SetHasNavigationBar(this, true);

            var nameLabel = new Label { Text = "Name :  " };
            var nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "Name");

            var addressLabel = new Label { Text = "Type : " };
            var addressEntry = new Entry();
            addressEntry.SetBinding(Entry.TextProperty, "Type");

            var portLabel = new Label { Text = "Enabled :" };
            var portEntry = new Entry();
            portEntry.SetBinding(Entry.TextProperty, "Enabled");
            
            
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
                    addressLabel, addressEntry,
                    portLabel, portEntry,
                    saveButton
                }
            };

        }

    }


}
