using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{

    public class SiteListPage : ContentPage
    {
        public ISiteRepository Repository { get; set; }
        

        private void SaveSite()
        {
            //load data - binding
            //AlarmSystemConfigurationContext.Site.authPasswd = passwordEntry.


            //validate


            //save
            Repository?.Update(AlarmSystemConfigurationContext.Site);
        }

        private void RemoveSite()
        {
            Repository?.Delete(AlarmSystemConfigurationContext.Site);
        }

        public SiteListPage()
        {
            this.SetBinding(ContentPage.TitleProperty, "siteName");

            Repository = AlarmSystemConfigurationContext.Repository;

            NavigationPage.SetHasNavigationBar(this, true);

             //   NavigationPage.BarBackgroundColorProperty	
            //   NavigationPage.BackgroundColorProperty. //= new BindableProperty(); Color.Aqua;
            //   NavigationPage.OpacityProperty = 0.5;

            var nameLabel = new Label { Text = "Name" };
            var nameEntry = new Entry();
            nameEntry.SetBinding(Entry.TextProperty, "siteName");

            var addressLabel = new Label { Text = "Hostname" };
            var addressEntry = new Entry();
            addressEntry.SetBinding(Entry.TextProperty, "serverIP");
            
            var portLabel = new Label { Text = "Port" };
            var portEntry = new Entry();
            portEntry.SetBinding(Entry.TextProperty, "serverPort");
            
            //var loginLabel = new Label { Text = "siteName" };
            //var loginEntry = new Entry();
            //loginEntry.SetBinding(Entry.TextProperty, "siteName");

            var passwordLabel = new Label { Text = "Password" };
            var passwordEntry = new Entry();
            passwordEntry.SetBinding(Entry.TextProperty, "authPasswd");


            var deleteButton = new Button { Text = "Remove" };
            deleteButton.Clicked += (sender, e) =>
            {
                RemoveSite();
                Navigation.PopAsync();
            };

            var saveButton = new Button { Text = "Save" };
            saveButton.Clicked += (sender, e) => {

                SaveSite();
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
                    passwordLabel, passwordEntry,
                    deleteButton,saveButton
                }
            };

        }

    }

}
