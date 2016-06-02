using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;

namespace sconnConnector.Config
{

    public interface IAlarmConfigManager
    {
        sconnAlarmSystem Config { get; set; }
        Device RemoteDevice { get; set; }
        EndpointInfo info { get; set; }
        DeviceCredentials creds { get; set; }
        DateTime LastUpDateTime { get; set; }
    }
    

    [Export(typeof(IAlarmConfigManager))]
    public class AlarmSystemConfigManager : BindableBase, IAlarmConfigManager
    {
        public EndpointInfo info { get; set; }
        public DeviceCredentials creds { get; set; }
        public Device RemoteDevice { get; set; }

        
        /****** Update interval -  cannot connect to remote device more often then specified    ******/
        public int MinUpdatePeriod { get; set; }
        public DateTime LastUpDateTime { get; set; }

        /****** Configuration of remote alarm system after processing  ********/
        private sconnAlarmSystem _config;
        public sconnAlarmSystem Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            } 
            
        }


        public List<sconnDevice> Type { get; set; }


        public void CopyFrom(AlarmSystemConfigManager other)
        {
            this.info.CopyFrom(other.info);
            this.creds.CopyFrom(other.creds);
            this.RemoteDevice.CopyFrom(other.RemoteDevice);
        }

        public AlarmSystemConfigManager()
        {
            Config = new sconnAlarmSystem();
            info = new EndpointInfo();
            creds = new DeviceCredentials();
            RemoteDevice = new Device();
            MinUpdatePeriod = 500;
        }

        public AlarmSystemConfigManager(EndpointInfo endp, DeviceCredentials cred) : this()
        {
            info = endp;
            creds = cred;
        }

        public AlarmSystemConfigManager(Device dev) : this(dev.EndpInfo,dev.Credentials)
        {
            this.RemoteDevice = dev;
        }


        private bool CanUpdateDueToTimingContraints()
        {
            return (DateTime.Now - LastUpDateTime).TotalMilliseconds > MinUpdatePeriod;
        }


        public int GetDeviceNumber()
        {
            return 1;
        }
    }
}
