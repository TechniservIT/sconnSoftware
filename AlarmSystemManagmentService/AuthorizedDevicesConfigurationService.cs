using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace AlarmSystemManagmentService
{
    public class AuthorizedDevicesConfigurationService
    {
        private IIotContextBase context;

        public AlarmSystemConfigManager Manager { get; set; }
        
        public AuthorizedDevicesConfigurationService(IIotContextBase cont)
        {
            this.context = cont;
        }
        
        public AuthorizedDevicesConfigurationService(IIotContextBase cont, Device AlarmDevice) : this(cont)
        {
            Manager = new AlarmSystemConfigManager(AlarmDevice.EndpInfo, AlarmDevice.Credentials);
        }

        public AuthorizedDevicesConfigurationService(IIotContextBase cont, AlarmSystemConfigManager man) : this(cont)
        {



        Manager = man;
        }

        public sconnAuthorizedDevices GetAuthorizedDevices()
        {
            return Manager.Config.AuthorizedDevices;
        }

        public sconnAuthorizedDevices GetAuthorizedDevicesAsync()
        {
            return Manager.Config.AuthorizedDevices;
        }

        public void AddAuthorizedDevice(sconnAuthorizedDevice device)
        {
            Manager.Config.AuthorizedDevices.Devices.Add(device);
        }

        public async Task AddAuthorizedDeviceAsync(sconnAuthorizedDevice device)
        {
            Manager.Config.AuthorizedDevices.Devices.Add(device);
            await Manager.UploadSiteConfigAsync();
        }

    }

}
