using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config.sconn;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Device
{

    public partial class AlarmDeviceEditView : ContentPage
    {
        public sconnDevice Config { get; set; }

        private void SaveSite()
        {
            //load data - binding

            //validate

            //save
        }


        public AlarmDeviceEditView()
        {
            this.SetBinding(ContentPage.TitleProperty, "Name");

            NavigationPage.SetHasNavigationBar(this, true);


            var typeButton = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            typeButton.SetBinding(Button.ImageProperty, "imageIconUri");


            var nameLabel = new Label { Text = "Name :  " };
            var nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "Name");

            var propertyLabel1 = new Label { Text = "Type : " };
            var propertyEntry1 = new Label();
            propertyEntry1.SetBinding(Label.TextProperty, "Type");

            var propertyLabel2 = new Label { Text = "Id : " };
            var propertyEntry2 = new Label();
            propertyEntry2.SetBinding(Label.TextProperty, "DeviceId");

            var propertyLabel3 = new Label { Text = "Inputs : " };
            var propertyEntry3 = new Label();
            propertyEntry3.SetBinding(Label.TextProperty, "InputNo");

            var propertyLabel4 = new Label { Text = "Outputs : " };
            var propertyEntry4 = new Label();
            propertyEntry4.SetBinding(Label.TextProperty, "OutputNo");

            var propertyLabel5 = new Label { Text = "Relays : " };
            var propertyEntry5 = new Label();
            propertyEntry5.SetBinding(Label.TextProperty, "RelayNo");




            var propertyLabel6 = new Label { Text = "Main supply voltage : " };
            var propertyEntry6 = new Label();
            propertyEntry6.SetBinding(Label.TextProperty, "MainVoltage");

            var propertyLabel7 = new Label { Text = "Backup supply voltage : " };
            var propertyEntry7 = new Label();
            propertyEntry7.SetBinding(Label.TextProperty, "BatteryVoltage");
            
            var propertyLabel8 = new Label { Text = "Revision : " };
            var propertyEntry8 = new Label();
            propertyEntry8.SetBinding(Label.TextProperty, "Revision");

            

            //public byte DeviceId { get; set; }
            //       public byte InputNo { get; set; }
            //       public byte OutputNo { get; set; }
            //       public byte RelayNo { get; set; }

            //       public float MainVoltage { get; set; }
            //       public float BatteryVoltage { get; set; }

            //       public bool KeypadModule { get; set; }
            //       public bool TemperatureModule { get; set; }
            //       public bool HumidityModule { get; set; }
            //       public bool ComMiWi { get; set; }
            //       public bool ComTcpIp { get; set; }
            //       public byte DomainNumber { get; set; }
            //       public byte Revision { get; set; }

            //       public string Name { get; set; }

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
                    typeButton,
                    nameLabel, nameEntry,
                    propertyLabel1, propertyEntry1,
                    propertyLabel2, propertyEntry2,
                    propertyLabel3, propertyEntry3,
                    propertyLabel4, propertyEntry4,
                    propertyLabel5, propertyEntry5,
                    propertyLabel6, propertyEntry6,
                    propertyLabel7, propertyEntry7,
                    propertyLabel8, propertyEntry8,
                    saveButton
                }
            };

        }

    }



}
