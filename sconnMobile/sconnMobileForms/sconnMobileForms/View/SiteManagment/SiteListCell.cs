using System;
using System.Collections.Generic;
using System.Text;
using iotDbConnector.DAL;
using sconnConnector.POCO.Config;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.View.AlarmSystem;
using SiteManagmentService;
using Xamarin.Forms;

namespace sconnMobileForms.View.SiteManagment
{

    public class SiteCellDisclousureClickedArgs : EventArgs
    {
        public sconnSite Site { get; set; }
    }


    public class SiteListCell : ViewCell
    {

        public event EventHandler<SiteCellDisclousureClickedArgs> ThresholdReached;



        public SiteListCell()
        {
            //Site = site;

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                YAlign = TextAlignment.Center
            };

            label.SetBinding(Label.TextProperty, "siteName");


            var saveButton = new Button
            {
                Image = "config2.png",
                HorizontalOptions = LayoutOptions.End
            };

            saveButton.Clicked += (sender, e) =>
            {
             //   var data = this.Parent.BindingContex//.BindingContext;
                sconnSite data = (sconnSite)BindingContext;
                AlarmSystemConfigurationContext.Site = data;
                var editPage = new SiteListPage { BindingContext = data };
                App.Navigation.PushAsync(editPage);
            };


            var grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 6, 8, 6)
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(0.8, GridUnitType.Star)});
                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(0.2, GridUnitType.Star)});  //GridLength.Auto});

                grid.Children.Add(label);
                grid.Children.Add(saveButton, 1, 0);
            
                View = grid;

            }

        }

}
