using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{

    public class SiteListPage : ContentPage
    {
        public SiteListPage()
        {
            this.SetBinding(ContentPage.TitleProperty, "siteName");

            NavigationPage.SetHasNavigationBar(this, true);

            var nameLabel = new Label { Text = "siteName" };
            var nameEntry = new Entry();

            nameEntry.SetBinding(Entry.TextProperty, "siteName");

            var deleteButton = new Button { Text = "Remove" };
            deleteButton.Clicked += (sender, e) => {
                var todoItem = (sconnSite)BindingContext;
                Navigation.PopModalAsync();
            };

            var saveButton = new Button { Text = "Save" };
            saveButton.Clicked += (sender, e) => {
                var todoItem = (sconnSite)BindingContext;
                Navigation.PopModalAsync();
            };


            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(20),
                Children = {
                    nameLabel, nameEntry,
                    deleteButton,saveButton
                }
            };

        }

    }

}
