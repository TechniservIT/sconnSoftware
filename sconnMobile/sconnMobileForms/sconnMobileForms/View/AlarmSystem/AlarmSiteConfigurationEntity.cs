using System;
using System.Collections.Generic;
using System.Text;
using sconnConnector.Config.Abstract;

namespace sconnMobileForms.View.AlarmSystem
{
    public class AlarmSiteConfigurationEntity
    {

        public AlarmSiteConfigurationEntity(string name, string uri, AlarmSystemConfigurationViewType type)
        {
            Name = name;
            Uri = uri;
            Type = type;
        }

        public string Name { get; set; }
        public string Uri { get; set; }
        public AlarmSystemConfigurationViewType Type { get; set; }
    }
}
