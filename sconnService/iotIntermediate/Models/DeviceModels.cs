﻿using iodash.Models.Common;
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
            ApplicationDbContext cont = new ApplicationDbContext();
            Locations = (from l in cont.Locations
                         select l).ToList();
            Types = (from t in cont.Types
                     select t).ToList();

        }

        public DeviceAddModel(Site site) : this()
        {
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