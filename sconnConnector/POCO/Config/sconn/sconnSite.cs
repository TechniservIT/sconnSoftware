using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{


    public class sconnSite
    {
        private int _statusCheckInterval;
        private string _authPasswd;
        private string _serverIP;
        private int _serverPort;
        private bool _ViewEnabled;
        private int _siteID;
        private string _siteName;
        private int SelectedTabId = 0;
        
        public bool UsbCom;
        
        public SiteConnectionStat siteStat;

        public ipcSiteConfig siteCfg;

        public string siteName
        {
            get { return _siteName; }
            set { _siteName = value; }
        }
        public string serverIP
        {
            get { return _serverIP; }
            set { _serverIP = value; }
        }

        public int serverPort
        {
            get { return _serverPort; }
            set { _serverPort = value; }
        }

        public bool ViewEnable
        {
            get { return _ViewEnabled; }
            set { _ViewEnabled = value; }
        }

        public int statusCheckInterval
        {
            get { return _statusCheckInterval; }
            set { _statusCheckInterval = value; }
        }

        public string authPasswd
        {
            get { return _authPasswd; }
            set
            {
                if (passwordFormatComplies(value))
                {
                    _authPasswd = value;
                }

            }
        }

        private DateTime lastUpdate;
        private DateTime lastWrite;

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                if (value != null)
                {
                    lastUpdate = (DateTime)value;
                }
            }
        }
        public DateTime LastWrite { get { return lastWrite; } }


        private bool passwordFormatComplies(string password)
        {
            if (password != null)
            {
                if ((password.Length <= ipcDefines.AUTH_PASS_SIZE) &&
                        (password.Length > 0)
                    )
                //additional password restrictions
                { return true; }

            }
            return false;
        }

        public int siteID
        {
            get { return _siteID; }
            set
            {
                if (value > 0)
                {
                    _siteID = value;
                }
            }
        }

        public sconnSite()
        {
            statusCheckInterval = 5000; //5s interval
            lastUpdate = DateTime.Now;
            serverPort = 37222;
            _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = "DefaultSite";
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
        }

        public sconnSite(ipcSiteConfig config, int intervalMs, string siteName)
        {
            statusCheckInterval = intervalMs;
            siteCfg = config;
            _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = siteName;
            lastUpdate = DateTime.Now;
            serverPort = 37222;
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
        }


        public sconnSite(int intervalMs, string server, int port, string siteName)
        {
            statusCheckInterval = intervalMs;
            serverIP = server;
            serverPort = port;
            _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = siteName;
            lastUpdate = DateTime.Now;
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
        }

        public sconnSite(string siteName, int intervalMs, string server, int port, string password)
            : this(intervalMs, server, port, siteName)
        {
            _authPasswd = password;
        }

    }

}
