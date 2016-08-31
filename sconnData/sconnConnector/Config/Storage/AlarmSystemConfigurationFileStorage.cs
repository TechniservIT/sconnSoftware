using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sconnConnector.Config.Abstract;

namespace sconnConnector.Config.Storage
{

    [Export(typeof(IAlarmSystemConfigurationStorage))]
    public class AlarmSystemConfigurationFileStorage : IAlarmSystemConfigurationStorage
    {
        public AlarmSystemConfigManager GetConfigFromUri(string uri)
        {
            try
            {
                StreamReader fileReader;
                if (Directory.Exists(uri))
                {
                    fileReader = new StreamReader(uri);
                }
                else
                {
                    return null;
                }

                string json = fileReader.ReadToEnd();
                JsonSerializer serializer = new JsonSerializer();
                AlarmSystemConfigManager result = JsonConvert.DeserializeObject<AlarmSystemConfigManager>(json);
                fileReader.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool IsConfigUriCorrect(string uri)
        {
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(uri)))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;   
            }
        }

        public bool StorageConfigAtUri(AlarmSystemConfigManager config, string uri)
        {
            try
            {
                StreamWriter fileWriter;
                if (File.Exists(uri))
                {
                    fileWriter = new StreamWriter(uri);
                }
                else
                {
                    fileWriter = File.CreateText(uri);
                }

                if (fileWriter != null)
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(fileWriter, config);
                    fileWriter.Close();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
