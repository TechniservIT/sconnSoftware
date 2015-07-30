
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
    public class DeviceAddModel
    {
        public List<Location> Locations { get; set; }

        public List<DeviceType> Types { get; set; }


        public Site DeviceSite { get; set; }

        public int DeviceSiteId { get; set; }


        [DisplayName("Location")]
        public Location DeviceLocation { get; set; }

        public int LocationId { get; set; }

        public int DeviceTypeId { get; set; }

        [DisplayName("Type")]
        public DeviceType DeviceType { get; set; }

        [DisplayName("Name")]
        public string DeviceName { get; set; }

        [DisplayName("Hostname")]
        public string DeviceIpAddr { get; set; }

        [DisplayName("Port")]
        public int DeviceNetPort { get; set; }

        [DisplayName("Login")]
        public string DeviceLogin { get; set; }

        [DisplayName("Password")]
        public string DevicePassword { get; set; }

        [DisplayName("Virtual")]
        public bool  DeviceIsVirtual { get; set; }

        //TODO protocol list
        //public CommSconnProtocol DeviceProtocol { get; set; }
        [DisplayName("Protocol")]
        public string DeviceProtocolName { get; set; }


        public DeviceAddModel()
        {
        }
        
        public DeviceAddModel(Site site) : this()
        {
            Locations = site.Domain.Locations.ToList();
            Types = site.Domain.DeviceTypes.ToList();
            DeviceSite = site;
            DeviceSiteId = site.Id;
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


    public class DeviceEditModel
    {

        [Required]
        public Device Device { get; set; }


        [Required]
        public int LocationId { get; set; }


        [DisplayName("Location")]
        public Location Location { get; set; }


        public List<Location> Locations { get; set; }


        [Required]
        public int TypeId { get; set; }

        [DisplayName("Type")]
        public DeviceType DeviceType { get; set; }


        public List<DeviceType> Types { get; set; }


        public DeviceEditModel(Device device, List<Location> locs, List<DeviceType> types)
        {
            Device = device;
            Locations = locs;
            Types = types;
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