using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.Config
{
    public class AlarmSystemConfigManager
    {
        private EndpointInfo info;
        private DeviceCredentials creds;
        public Device RemoteDevice { get; set; }

        
        /****** Update interval -  cannot connect to remote device more often then specified    ******/
        public int MinUpdatePeriod { get; set; }
        public DateTime LastUpDateTime { get; set; }

        /****** Configuration of remote alarm system after processing  ********/
        public sconnAlarmSystem Config { get; set; }

        public AlarmSystemConfigManager()
        {
            Config = new sconnAlarmSystem();
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
