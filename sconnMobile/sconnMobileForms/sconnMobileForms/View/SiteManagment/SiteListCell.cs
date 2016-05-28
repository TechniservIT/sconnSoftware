using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{

    public class SiteListCell : ViewCell
    {
        public SiteListCell()
        {
            var label = new Label
            {
                //VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                   YAlign = TextAlignment.Center
            };
            
            label.SetBinding(Label.TextProperty, "siteName");

            //var saveButton = new Button
            //{
            //    Image = "add44.png",
            //    // VerticalOptions.Alignment = LayoutAlignment.End
            //    HorizontalOptions = LayoutOptions.EndAndExpand
            //};
            //saveButton.Clicked += (sender, e) => {

            //};

            var saveButton = new Image
            {
                Source = FileImageSource.FromFile("add44.png"),
                HorizontalOptions = LayoutOptions.EndAndExpand     // AndExpand
            };
            saveButton.SetBinding(Image.IsVisibleProperty, "Done");



            //  saveButton.ContentLayout.Position = Button.ButtonContentLayout.ImagePosition.Right;

            // saveButton.VerticalOptions.Alignment = LayoutAlignment.End;


            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { label, saveButton }
            };


            View = layout;
        }
    }


}
