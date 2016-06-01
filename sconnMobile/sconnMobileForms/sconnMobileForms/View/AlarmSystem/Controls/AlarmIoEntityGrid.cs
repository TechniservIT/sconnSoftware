using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnMobileForms.Service.AlarmSystem.Context;
using sconnMobileForms.Service.AlarmSystem.Io;
using Xamarin.Forms;

namespace sconnMobileForms.View.AlarmSystem.Controls
{

    public static class AlarmIoEntityHelpers
    {
        public static string AlarmIoEntityIconPathForType(AlarmSystemIoType type)
        {
            if (type == AlarmSystemIoType.Input)
            {
                return "eye1000.jpg";
            }
            else if (type == AlarmSystemIoType.Output)
            {
                return "elektro1000.jpg";
            }
            else if (type == AlarmSystemIoType.Relay)
            {
                return "przekaznik1000.jpg";
            }
            return "add.jpg";
        }
    }

    public class AlarmIoEntityGrid : Grid
    {
        public string Title { get; set; }
        public List<AlarmIoEntityGridItem> Items { get; set; }

        public AlarmIoEntityGrid()
        {
                Items=new List<AlarmIoEntityGridItem>();
                VerticalOptions = LayoutOptions.FillAndExpand;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            Padding = new Thickness(1, 1, 1, 1);
         
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });  
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
        }

        private void AddDescriptionRow()
        {
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start,
                Text = Title,
                FontFamily = "Copperplate"
            };
            Children.Add(label, 0, 0);
        }

        public void SetTitle(string title)
        {
            Title = title;
            ReloadGrid();
        }

        private void ReloadGrid()
        {
            RowDefinitions = new RowDefinitionCollection();
            AddDescriptionRow();

            for (int i = 0; i < Items.Count; i++)
            {
                if (i%2 == 0)
                {
                    RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star) });
                }
                
                Children.Add(Items[i], i%2 == 0 ? 0 : 1, i/2 + 1);  // t
            }
        }

        public AlarmIoEntityGrid(List<sconnInput> inputs) : this()
        {
            foreach (var input in inputs)
            {
                AlarmIoInputEntityGridItem item = new AlarmIoInputEntityGridItem(input);
                Items.Add(item);
            }
            ReloadGrid();
        }

        public AlarmIoEntityGrid(List<sconnOutput> outputs) : this()
        {
            foreach (var output in outputs)
            {
                AlarmIoOutputEntityGridItem item = new AlarmIoOutputEntityGridItem();
                Items.Add(item); 
            }
            ReloadGrid();
        }

        public AlarmIoEntityGrid(List<sconnRelay> relays) : this()
        {
            foreach (var relay in relays)
            {
                AlarmIoRelayEntityGridItem item = new AlarmIoRelayEntityGridItem();
                Items.Add(item);
            }
            ReloadGrid();
        }

        public AlarmIoEntityGrid(sconnDevice dev, AlarmSystemIoType type) : this()
        {
            if (type == AlarmSystemIoType.Input)
            {
                foreach (var input in dev.Inputs)
                {
                    AlarmIoInputEntityGridItem item = new AlarmIoInputEntityGridItem(input);
                    Items.Add(item);
                }
            }
            else if (type == AlarmSystemIoType.Output)
            {
                foreach (var output in dev.Outputs)
                {
                    IAlarmIoConfigService service =  new AlarmIoOutputConfigService(AlarmSystemConfigurationContext.AlarmConfigManager, dev, output);
                    AlarmIoOutputEntityGridItem item = new AlarmIoOutputEntityGridItem(service, output);
                    Items.Add(item);
                }
            }
            else if (type == AlarmSystemIoType.Relay)
            {
                foreach (var relay in dev.Relays)
                {
                    IAlarmIoConfigService service = new AlarmIoRelayConfigService(AlarmSystemConfigurationContext.AlarmConfigManager, dev, relay);
                    AlarmIoRelayEntityGridItem item = new AlarmIoRelayEntityGridItem(service, relay);
                    Items.Add(item);
                }
            }
            ReloadGrid();
        }

    }

}
