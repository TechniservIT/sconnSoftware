using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Annotations;

namespace sconnConnector.POCO.Config
{


    public class sconnSite : INotifyPropertyChanged 
    {
        private int _statusCheckInterval;
        private string _authPasswd;
        private string _serverIP;
        private int _serverPort;
        private bool _ViewEnabled;
        private int _siteID;
        private string _siteName;
        private int SelectedTabId = 0;

        public string UUID { get; set; }

        public string Id { get; set; }
        
        public bool UsbCom;

        private SiteConnectionStat _siteStat;
        public SiteConnectionStat SiteStat
        {
            get { return _siteStat; }
            set
            {
                _siteStat = value;
                OnPropertyChanged();
            }
        }

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

        public void CopyFrom(sconnSite otherSite)
        {
            try
            {
                this.siteName = otherSite.siteName;
                this.serverIP = otherSite.serverIP;
                this.serverPort = otherSite.serverPort;
                this.siteCfg = otherSite.siteCfg;
                OnPropertyChanged();
            }
            catch (Exception)
            {
            }
        }

        public sconnSite(sconnSite otherSite) :this()
        {
            this.siteName = otherSite.siteName;
            this.serverIP = otherSite.serverIP;
            this.serverPort = otherSite.serverPort;
            this.siteCfg = otherSite.siteCfg;
        }

        public sconnSite()
        {
            UUID = Guid.NewGuid().ToString();

            statusCheckInterval = 5000; //5s interval
            lastUpdate = DateTime.Now;
            serverPort = 37222;
       //     _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = "DefaultSite";
            siteCfg = new ipcSiteConfig();
            _siteStat = new SiteConnectionStat();
            Id = Guid.NewGuid().ToString();
        }

        public sconnSite(ipcSiteConfig config, int intervalMs, string siteName)
        {

            UUID = new Guid().ToString();

            statusCheckInterval = intervalMs;
            siteCfg = config;
         //   _siteID = sconnDataShare.GetLastItemID() + 1;
            _siteName = siteName;
            lastUpdate = DateTime.Now;
            serverPort = 37222;
            siteCfg = new ipcSiteConfig();
            _siteStat = new SiteConnectionStat();
            Id = Guid.NewGuid().ToString();
        }


        public sconnSite(int intervalMs, string server, int port, string siteName)
        {

            UUID = new Guid().ToString();

            statusCheckInterval = intervalMs;
            serverIP = server;
            serverPort = port;
            _siteName = siteName;
            lastUpdate = DateTime.Now;
            siteCfg = new ipcSiteConfig();
            _siteStat = new SiteConnectionStat();
            Id = Guid.NewGuid().ToString();
        }

        public sconnSite(string siteName, int intervalMs, string server, int port, string password)
            : this(intervalMs, server, port, siteName)
        {
            _authPasswd = password;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
