
using iotDash.Session;
using iotDbConnector.DAL;
using iotDeviceService;
using iotServiceProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace iotDash.Models
{
    public class DeviceAddModel
    {
        public List<Location> Locations { get; set; }

        public List<DeviceType> Types { get; set; }

        public Site DeviceSite { get; set; }


        public DeviceAddModel()
        {
        }
        
        public DeviceAddModel(Site site) : this()
        {
            Locations = site.Domain.Locations.ToList();
            Types = site.Domain.DeviceTypes.ToList();
            DeviceSite = site;
        }
    }

    public class DeviceListViewModel
    {
        public Site Site { get; set; }
        public DeviceListViewModel(Site site)
        {
            Site = site;
        }

        public string SiteLatCordStr()
        {
            return Site.siteLocation.Lat.ToString(CultureInfo.InvariantCulture);
        }

        public string SiteLngCordStr()
        {
            return Site.siteLocation.Lng.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class DeviceViewModel
    {
        public Device Device { get; set; }
        public DeviceViewModel(Device device)
        {
            Device = device;
        }

        public string DeviceLatCordStr()
        {
            return Device.DeviceLocation.Lat.ToString(CultureInfo.InvariantCulture);
        }

        public string DeviceLngCordStr()
        {
            return Device.DeviceLocation.Lng.ToString(CultureInfo.InvariantCulture);
        }

    }


}