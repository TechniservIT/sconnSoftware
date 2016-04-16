﻿
using iotDash.Session;
using iotDbConnector.DAL;
using iotServiceProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace iotDash.Models
{
    public class DeviceAddTypeModel : IAsyncStatusModel
    {

        [DisplayName("Type")]
        public DeviceType Type { get; set; }

        public string Result { get; set; }

        public DeviceAddTypeModel()
        {
                
        }

    }

    public class DeviceTypesListModel : IAsyncStatusModel
    {

        [DisplayName("Type")]
        public List<DeviceType> Types { get; set; }

        public string Result { get; set; }

        public DeviceTypesListModel()
        {
            Types = new List<DeviceType>();
        }

        public DeviceTypesListModel(List<DeviceType> types) : this()
        {
            Types = types;
        }

    }

    public class DeviceAddModel : IAsyncStatusModel
    {
        [DisplayName("Site")]
        public Site DeviceSite { get; set; }

        public List<Location> Locations { get; set; }
        public List<DeviceType> Types { get; set; }

        public string Result { get; set; }
        
        [Required]
        [DisplayName("Site")]
        public int DeviceSiteId { get; set; }

        [Required]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        [Required]
        [DisplayName("Type")]
        public int TypeId { get; set; }

        [Required]
        [DisplayName("Device")]
        public Device Device { get; set; }
        
        //TODO protocol list
        //public CommSconnProtocol DeviceProtocol { get; set; }
        //[DisplayName("Protocol")]
        //public string DeviceProtocolName { get; set; }
        
        public DeviceAddModel()
        {
            this.Device = new Device();
            Device.EndpInfo = new EndpointInfo();
            Device.Credentials = new DeviceCredentials();
            Device.DeviceLocation = new Location();
            Device.Type = new DeviceType();

            Locations = new List<Location>();
            Types = new List<DeviceType>();
            DeviceSite = new Site();

        }
        
        public DeviceAddModel(Site site) : this()
        {
            Locations = site.Domain.Locations.ToList();
            Types = site.Domain.DeviceTypes.ToList();
            DeviceSite = site;
            DeviceSiteId = site.Id;
        }
    }

    public class DeviceListViewModel : IAsyncStatusModel
    {
        public string Result { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public List<Device> Devices { get; set; }

        public DeviceListViewModel(Site site)
        {
            Devices = site.Devices;
            this.SiteName = site.SiteName;
            this.SiteId = site.Id;
        }

        public DeviceListViewModel(List<Device> devices)
        {
            Devices = devices;
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


    public class DeviceEditModel : IAsyncStatusModel
    {
        [Required]
        public Device Device { get; set; }

        public List<Location> Locations { get; set; }
        public string Result { get; set; }
        public List<DeviceType> Types { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public int TypeId { get; set; }

        public DeviceEditModel(Device device, List<Location> locs, List<DeviceType> types)
        {
            Device = device;
            Locations = locs;
            Types = types;
        }

        public DeviceEditModel()
        {
                
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