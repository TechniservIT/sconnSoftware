using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Annotations;
using SQLite;

namespace sconnConnector.POCO.Config
{


    [Table("Sites")]
    public class sconnSite : INotifyPropertyChanged
    {
        private string _authPasswd;
        private int _siteID;
        private int SelectedTabId = 0;

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        
        public string UniqueId { get; set; }
        public DateTime LastWrite { get; }
        public bool UsbCom { get; set; }
       
        public SiteConnectionStat siteStat;
        public ipcSiteConfig siteCfg;

        public string siteName { get; set; }

        public string serverIP { get; set; }

        public int serverPort { get; set; }

        public bool ViewEnable { get; set; }

        public int statusCheckInterval { get; set; }

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
            statusCheckInterval = 5000; //5s interval
            lastUpdate = DateTime.Now;
            serverPort = 37222;
       //     _siteID = sconnDataShare.GetLastItemID() + 1;
            siteName = "DefaultSite";
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
            UniqueId = Guid.NewGuid().ToString();
        }

        public sconnSite(ipcSiteConfig config, int intervalMs, string siteName)
        {
            statusCheckInterval = intervalMs;
            siteCfg = config;
         //   _siteID = sconnDataShare.GetLastItemID() + 1;
            this.siteName = siteName;
            lastUpdate = DateTime.Now;
            serverPort = 37222;
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
            UniqueId = Guid.NewGuid().ToString();
        }


        public sconnSite(int intervalMs, string server, int port, string siteName)
        {
            statusCheckInterval = intervalMs;
            serverIP = server;
            serverPort = port;
       //     _siteID = sconnDataShare.GetLastItemID() + 1;
            this.siteName = siteName;
            lastUpdate = DateTime.Now;
            siteCfg = new ipcSiteConfig();
            siteStat = new SiteConnectionStat();
            UniqueId = Guid.NewGuid().ToString();
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
