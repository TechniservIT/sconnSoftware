using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnRem
{
    public class GlobalSettings
    {

        public string CultureName { get; set; }
        public int DefaultReloadInterval { get; set; }
        public string ConfigFilePath { get; set; }


        public GlobalSettings(string Culture, int ReloadInterval, string ConfigPath)
        {
            CultureName = Culture;
            DefaultReloadInterval = ReloadInterval;
            ConfigFilePath = ConfigPath;
        }

        public GlobalSettings()
        {
            CultureName = "en-US";
            DefaultReloadInterval = 10000;
            ConfigFilePath =  System.Environment.SpecialFolder.SystemX86 + "/" + "sconnRem";
        }

    }


}
