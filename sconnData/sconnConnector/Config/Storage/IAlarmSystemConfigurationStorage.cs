using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config.Abstract;

namespace sconnConnector.Config.Storage
{
    public interface IAlarmSystemConfigurationStorage
    {
        AlarmSystemConfigManager GetConfigFromUri(string uri);
        bool IsConfigUriCorrect(string uri);
        bool StorageConfigAtUri(AlarmSystemConfigManager config, string uri);
    }

}
